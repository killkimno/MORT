using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MORT.Service.Overlay
{
    internal readonly record struct OverlayColorAnalysis(
        bool Success,
        Color Font,
        Color Background,
        bool UsedFontFallback,
        int ForegroundWordSupport,
        double ForegroundContrast);

    internal static class OverlayColorAnalyzer
    {
        private const int MaximumBackgroundSamples = 65536;
        private const int MaximumWordSamples = 4096;
        private const double MinimumForegroundContrast = 2.5;
        private const double BackgroundRingPaddingRatio = 0.35;
        private const int MinimumBackgroundRingPadding = 2;
        private const int MaximumBackgroundRingPadding = 12;

        public static OverlayColorAnalysis Analyze(
            byte[] source,
            int channels,
            int width,
            int height,
            Rectangle sourceRect,
            IReadOnlyList<Rectangle> wordRects)
        {
            Rectangle imageRect = new Rectangle(0, 0, Math.Max(0, width), Math.Max(0, height));
            sourceRect = Rectangle.Intersect(sourceRect, imageRect);
            if(source == null || sourceRect.Width <= 0 || sourceRect.Height <= 0)
            {
                return new OverlayColorAnalysis(false, Color.Empty, Color.Empty, true, 0, 0);
            }

            var candidates = new Dictionary<int, ForegroundCluster>();
            var sampleRects = wordRects?
                .Select(rect => Rectangle.Intersect(rect, imageRect))
                .Where(rect => rect.Width > 0 && rect.Height > 0)
                .ToList() ?? new List<Rectangle>();
            bool hasWordRects = sampleRects.Count > 0;
            if(sampleRects.Count == 0)
            {
                sampleRects.Add(sourceRect);
            }

            if(!TryFindBackgroundFromWordRings(
                    source,
                    channels,
                    width,
                    height,
                    sampleRects,
                    hasWordRects,
                    out Color background)
                && !TryFindDominantColor(
                    source,
                    channels,
                    width,
                    height,
                    sourceRect,
                    MaximumBackgroundSamples,
                    out background))
            {
                return new OverlayColorAnalysis(false, Color.Empty, Color.Empty, true, 0, 0);
            }

            for(int wordIndex = 0; wordIndex < sampleRects.Count; wordIndex++)
            {
                Rectangle wordRect = sampleRects[wordIndex];
                Color localBackground = background;
                if(hasWordRects && TryFindRingDominantColor(
                    source,
                    channels,
                    width,
                    height,
                    wordRect,
                    sampleRects,
                    out Color ringBackground))
                {
                    localBackground = ringBackground;
                }

                int stride = GetStride(wordRect, MaximumWordSamples);
                var supportedKeys = new HashSet<int>();
                VisitPixels(source, channels, width, height, wordRect, stride, color =>
                {
                    double contrast = GetContrastRatio(color, localBackground);
                    if(contrast < MinimumForegroundContrast)
                    {
                        return;
                    }

                    int key = GetQuantizedKey(color);
                    if(!candidates.TryGetValue(key, out ForegroundCluster? cluster))
                    {
                        cluster = new ForegroundCluster();
                        candidates.Add(key, cluster);
                    }

                    cluster.Add(color, contrast);
                    supportedKeys.Add(key);
                });

                foreach(int key in supportedKeys)
                {
                    candidates[key].WordSupport++;
                }
            }

            ForegroundCluster? selected = candidates.Values
                .OrderByDescending(cluster => cluster.WordSupport)
                .ThenByDescending(cluster => cluster.Population)
                .ThenByDescending(cluster => cluster.AverageContrast)
                .FirstOrDefault();
            if(selected == null)
            {
                Color fallback = GetContrastingColor(background);
                return new OverlayColorAnalysis(
                    true,
                    fallback,
                    background,
                    true,
                    0,
                    GetContrastRatio(fallback, background));
            }

            Color font = selected.GetMedianColor();
            return new OverlayColorAnalysis(
                true,
                font,
                background,
                false,
                selected.WordSupport,
                GetContrastRatio(font, background));
        }

        private static bool TryFindBackgroundFromWordRings(
            byte[] source,
            int channels,
            int width,
            int height,
            IReadOnlyList<Rectangle> wordRects,
            bool hasWordRects,
            out Color result)
        {
            result = Color.Empty;
            if(!hasWordRects)
            {
                return false;
            }

            var clusters = new Dictionary<int, ForegroundCluster>();
            for(int wordIndex = 0; wordIndex < wordRects.Count; wordIndex++)
            {
                Rectangle ringRect = GetBackgroundRingRect(wordRects[wordIndex], width, height);
                int stride = GetStride(ringRect, MaximumWordSamples);
                var supportedKeys = new HashSet<int>();
                VisitPixels(source, channels, width, height, ringRect, stride, color =>
                {
                    int key = GetQuantizedKey(color);
                    if(!clusters.TryGetValue(key, out ForegroundCluster? cluster))
                    {
                        cluster = new ForegroundCluster();
                        clusters.Add(key, cluster);
                    }

                    cluster.Add(color, 0);
                    supportedKeys.Add(key);
                }, (x, y) => !ContainsAny(wordRects, x, y));

                foreach(int key in supportedKeys)
                {
                    clusters[key].WordSupport++;
                }
            }

            ForegroundCluster? selected = clusters.Values
                .OrderByDescending(cluster => cluster.WordSupport)
                .ThenByDescending(cluster => cluster.Population)
                .FirstOrDefault();
            if(selected == null)
            {
                return false;
            }

            result = selected.GetMedianColor();
            return true;
        }

        private static bool TryFindRingDominantColor(
            byte[] source,
            int channels,
            int width,
            int height,
            Rectangle wordRect,
            IReadOnlyList<Rectangle> wordRects,
            out Color result)
        {
            Rectangle ringRect = GetBackgroundRingRect(wordRect, width, height);
            return TryFindDominantColor(
                source,
                channels,
                width,
                height,
                ringRect,
                MaximumWordSamples,
                out result,
                (x, y) => !ContainsAny(wordRects, x, y));
        }

        private static Rectangle GetBackgroundRingRect(Rectangle wordRect, int width, int height)
        {
            int shortSide = Math.Max(1, Math.Min(wordRect.Width, wordRect.Height));
            int padding = Math.Clamp(
                (int)Math.Ceiling(shortSide * BackgroundRingPaddingRatio),
                MinimumBackgroundRingPadding,
                MaximumBackgroundRingPadding);
            Rectangle expanded = Rectangle.Inflate(wordRect, padding, padding);
            return Rectangle.Intersect(expanded, new Rectangle(0, 0, width, height));
        }

        private static bool ContainsAny(IReadOnlyList<Rectangle> rects, int x, int y)
        {
            for(int index = 0; index < rects.Count; index++)
            {
                if(rects[index].Contains(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryFindDominantColor(
            byte[] source,
            int channels,
            int width,
            int height,
            Rectangle rect,
            int maximumSamples,
            out Color result,
            Func<int, int, bool>? includePixel = null)
        {
            result = Color.Empty;
            int stride = GetStride(rect, maximumSamples);
            var counts = new Dictionary<int, int>();
            VisitPixels(source, channels, width, height, rect, stride, color =>
            {
                int key = GetQuantizedKey(color);
                counts.TryGetValue(key, out int count);
                counts[key] = count + 1;
            }, includePixel);
            if(counts.Count == 0)
            {
                return false;
            }

            int dominantKey = counts
                .OrderByDescending(item => item.Value)
                .ThenBy(item => item.Key)
                .First().Key;
            var red = new List<byte>(counts[dominantKey]);
            var green = new List<byte>(counts[dominantKey]);
            var blue = new List<byte>(counts[dominantKey]);
            VisitPixels(source, channels, width, height, rect, stride, color =>
            {
                if(GetQuantizedKey(color) != dominantKey)
                {
                    return;
                }

                red.Add(color.R);
                green.Add(color.G);
                blue.Add(color.B);
            }, includePixel);
            if(red.Count == 0)
            {
                return false;
            }

            result = Color.FromArgb(GetMedian(red), GetMedian(green), GetMedian(blue));
            return true;
        }

        private static void VisitPixels(
            byte[] source,
            int channels,
            int width,
            int height,
            Rectangle rect,
            int stride,
            Action<Color> visitor,
            Func<int, int, bool>? includePixel = null)
        {
            if(source == null || channels <= 0 || width <= 0 || height <= 0)
            {
                return;
            }

            Rectangle clipped = Rectangle.Intersect(rect, new Rectangle(0, 0, width, height));
            for(int y = clipped.Top; y < clipped.Bottom; y += stride)
            {
                for(int x = clipped.Left; x < clipped.Right; x += stride)
                {
                    if(includePixel != null && !includePixel(x, y))
                    {
                        continue;
                    }

                    long offset = ((long)y * width + x) * channels;
                    if(offset < 0 || offset + channels > source.LongLength)
                    {
                        continue;
                    }

                    int index = (int)offset;
                    byte red;
                    byte green;
                    byte blue;
                    byte alpha = 255;
                    if(channels == 1)
                    {
                        red = green = blue = source[index];
                    }
                    else if(channels >= 3)
                    {
                        blue = source[index];
                        green = source[index + 1];
                        red = source[index + 2];
                        if(channels >= 4)
                        {
                            alpha = source[index + 3];
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if(alpha >= 128)
                    {
                        visitor(Color.FromArgb(red, green, blue));
                    }
                }
            }
        }

        private static int GetStride(Rectangle rect, int maximumSamples)
        {
            long area = Math.Max(1L, (long)rect.Width * rect.Height);
            return Math.Max(1, (int)Math.Ceiling(Math.Sqrt(area / (double)Math.Max(1, maximumSamples))));
        }

        private static int GetQuantizedKey(Color color)
        {
            return ((color.R >> 3) << 10) | ((color.G >> 3) << 5) | (color.B >> 3);
        }

        private static byte GetMedian(List<byte> values)
        {
            values.Sort();
            int middle = values.Count / 2;
            return values.Count % 2 == 1
                ? values[middle]
                : (byte)((values[middle - 1] + values[middle]) / 2);
        }

        private static Color GetContrastingColor(Color background)
        {
            Color black = Color.Black;
            Color white = Color.White;
            return GetContrastRatio(black, background) >= GetContrastRatio(white, background)
                ? black
                : white;
        }

        private static double GetContrastRatio(Color first, Color second)
        {
            double firstLuminance = GetRelativeLuminance(first);
            double secondLuminance = GetRelativeLuminance(second);
            double lighter = Math.Max(firstLuminance, secondLuminance);
            double darker = Math.Min(firstLuminance, secondLuminance);
            return (lighter + 0.05) / (darker + 0.05);
        }

        private static double GetRelativeLuminance(Color color)
        {
            return 0.2126 * ToLinear(color.R)
                + 0.7152 * ToLinear(color.G)
                + 0.0722 * ToLinear(color.B);
        }

        private static double ToLinear(byte channel)
        {
            double value = channel / 255.0;
            return value <= 0.04045
                ? value / 12.92
                : Math.Pow((value + 0.055) / 1.055, 2.4);
        }

        private sealed class ForegroundCluster
        {
            private readonly List<byte> _red = new();
            private readonly List<byte> _green = new();
            private readonly List<byte> _blue = new();
            private double _contrastSum;

            public int WordSupport { get; set; }
            public int Population => _red.Count;
            public double AverageContrast => Population == 0 ? 0 : _contrastSum / Population;

            public void Add(Color color, double contrast)
            {
                _red.Add(color.R);
                _green.Add(color.G);
                _blue.Add(color.B);
                _contrastSum += contrast;
            }

            public Color GetMedianColor()
            {
                return Color.FromArgb(GetMedian(_red), GetMedian(_green), GetMedian(_blue));
            }
        }
    }
}
