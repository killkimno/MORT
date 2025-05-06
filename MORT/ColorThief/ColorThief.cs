using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
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

        public static IEnumerable<QuantizedColor> GetPalette(in byte[] sourceImage, int channels, int originalX, int originalY, Rectangle rect, int colorCount = DefaultColorCount, int quality = DefaultQuality, bool ignoreWhite = DefaultIgnoreWhite)
        {

            if(quality < 1)
            {
                quality = DefaultQuality;
            }
            var pixelCount = rect.Width * rect.Height;
            var numRegardedPixels = (pixelCount + quality - 1) / quality;


            var result = GetPixelsFast(sourceImage, originalX, originalY, rect, channels, quality, ignoreWhite);
            var cmap = Mmcq.Quantize(result, colorCount);

            return cmap.GeneratePalette();
        }

        static private byte[][] GetPixelsFast(in byte[] imageData, int originalX, int originalY, Rectangle rect, int channels, int quality, bool ignoreWhite)
        {
            var pixels = imageData;
            var pixelCount = rect.Width * rect.Height;


            // Store the RGB values in an array format suitable for quantize
            // function

            // numRegardedPixels must be rounded up to avoid an
            // ArrayIndexOutOfBoundsException if all pixels are good.

            var numRegardedPixels = (quality <= 0) ? 0 : (rect.Height * (int)Math.Ceiling((double)rect.Width / quality));

            var numUsedPixels = 0;
            var pixelArray = new byte[numRegardedPixels][];

            if(channels == 4)
            {

                for(int y = 0; y < rect.Height; y++)
                {
                    int shiftY = (rect.Top + y) * originalX;

                    for(int x = 0; x < rect.Width; x += quality)
                    {
                        int shiftX = rect.Left + x;                        
                        
                        var offset = (shiftY + shiftX) * channels;

                        var b = pixels[offset];
                        var g = pixels[offset + 1];
                        var r = pixels[offset + 2];
                        var a = pixels[offset + 3]; //a가 필요한가?
                        // If pixel is mostly opaque and not white
                        if(a >= 125 && !(ignoreWhite && r > 250 && g > 250 && b > 250))
                        {
                            pixelArray[numUsedPixels] = new[] { r, g, b };
                            numUsedPixels++;
                        }
                    }
                }

            }
            else if(channels == 3)
            {

                for(int y = 0; y < rect.Height; y++)
                {
                    int shiftY = (rect.Top + y) * originalX;

                    for(int x = 0; x < rect.Width; x += quality)
                    {
                        int shiftX = rect.Left + x;

                        var offset = (shiftY + shiftX) * channels;
                        var b = pixels[offset];
                        var g = pixels[offset + 1];
                        var r = pixels[offset + 2];

                        if(!(ignoreWhite && r > 250 && g > 250 && b > 250))
                        {
                            pixelArray[numUsedPixels] = new[] { r, g, b };
                            numUsedPixels++;
                        }
                    }
                }
            }

            // Remove unused pixels from the array
            var copy = new byte[numUsedPixels][];
            Array.Copy(pixelArray, copy, numUsedPixels);
            return copy;
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
