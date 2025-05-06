using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Graphics.Imaging;
using WinRT;

namespace MORT.ColorThief
{
    public record QuantizedColor(Color Color, int Population)
    {
        public bool IsDark { get; } = CalculateYiqLuma(Color) < 128;

        private static int CalculateYiqLuma(Color color) => Convert.ToInt32(Math.Round((299 * color.R + 587 * color.G + 114 * color.B) / 1000f));
    }

    //TODO : https://github.com/KSemenenko/ColorThief 이걸로 교체하는걸 고려해본다

    public static class ColorThief
    {
        public const int DefaultColorCount = 5;
        public const int DefaultQuality = 10;
        public const bool DefaultIgnoreWhite = true;
        public const int ColorDepth = 4;

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="colorCount">The color count.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns></returns>
        /// <code>true</code>
        public static IEnumerable<QuantizedColor> GetPalette(SoftwareBitmap sourceImage, int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
            => GetPalette(sourceImage, Rectangle.Empty, colorCount, quality, ignoreWhite);

        /// <summary>
        ///     Use the median cut algorithm to cluster similar colors.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="colorCount">The color count.</param>
        /// <param name="quality">
        ///     1 is the highest quality settings. 10 is the default. There is
        ///     a trade-off between quality and speed. The bigger the number,
        ///     the faster a color will be returned but the greater the
        ///     likelihood that it will not be the visually most dominant color.
        /// </param>
        /// <param name="ignoreWhite">if set to <c>true</c> [ignore white].</param>
        /// <returns></returns>
        /// <code>true</code>
        public static IEnumerable<QuantizedColor> GetPalette(SoftwareBitmap sourceImage, Rectangle rect, int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {
            if(rect == Rectangle.Empty)
            {
                rect = new(0, 0, sourceImage.PixelWidth, sourceImage.PixelHeight);
            }
            if(quality < 1)
            {
                quality = DefaultQuality;
            }
            var pixelCount = rect.Width * rect.Height;
            var numRegardedPixels = (pixelCount + quality - 1) / quality;
            var buf = ArrayPool<byte>.Shared.Rent(numRegardedPixels * 3);
            var span = buf.AsSpan(0, numRegardedPixels * 3);

            var r = span[..numRegardedPixels];
            var g = span[numRegardedPixels..(numRegardedPixels * 2)];
            var b = span[(numRegardedPixels * 2)..(numRegardedPixels * 3)];

            GetPixelsFast(sourceImage, rect, quality, ignoreWhite, r, g, b);
            Simd.Rshift(span, Mmcq.Rshift);

            var cmap = Mmcq.Quantize(r, g, b, --colorCount);
            ArrayPool<byte>.Shared.Return(buf);
            return cmap.GeneratePalette();
        }

        unsafe private static void GetPixelsFast(SoftwareBitmap bmp, Rectangle rect, int quality, bool ignoreWhite, Span<byte> r, Span<byte> g, Span<byte> b)
        {
            using var buffer = bmp.LockBuffer(BitmapBufferAccessMode.Read);
            using var reference = buffer.CreateReference();
            reference.As<IMemoryBufferByteAccess>().GetBuffer(out var data, out _);

            var bufferLayout = buffer.GetPlaneDescription(0);
            const int bytesPerPixel = 4; // BGRA8

            var numUsedPixels = 0;

            var top = Math.Clamp(rect.Top, 0, bmp.PixelWidth);
            var bottom = Math.Clamp(rect.Bottom, 0, bmp.PixelHeight);
            var left = Math.Clamp(rect.Left, 0, bmp.PixelWidth);
            var width = Math.Clamp(rect.Right - left, 0, bmp.PixelWidth - left);

            var mod = 0;
            for(var y = top; y < bottom; y++)
            {
                int rowStartIndex = bufferLayout.StartIndex + (y * bufferLayout.Stride) + (left * bytesPerPixel);
                for(; mod < width; mod += quality)
                {
                    var pixelIndex = rowStartIndex + (mod * bytesPerPixel);
                    var pb = data[pixelIndex++];
                    var pg = data[pixelIndex++];
                    var pr = data[pixelIndex++];
                    var pa = data[pixelIndex++];

                    // If pixel is mostly opaque and not white
                    if(pa >= 125 && !(ignoreWhite && pr > 250 && pg > 250 && pb > 250))
                    {
                        r[numUsedPixels] = pr;
                        g[numUsedPixels] = pg;
                        b[numUsedPixels] = pb;
                        numUsedPixels++;
                    }
                }
                mod -= width;
            }
        }
    }

    [ComImport]
    [Guid("5B0D3235-4DBA-4D44-865E-8F1D0E4FD04D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    unsafe interface IMemoryBufferByteAccess
    {
        void GetBuffer(out byte* buffer, out uint capacity);
    }

    readonly record struct RGB(byte R, byte G, byte B);
}
