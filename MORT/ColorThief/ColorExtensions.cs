using System;
using System.Drawing;

namespace MORT.ColorThief
{
    public static class ColorExtensions
    {
        /// <summary>
        ///     Get HSL color.
        /// </summary>
        /// <returns></returns>
        public static HslColor ToHsl(this Color color)
        {
            const double toDouble = 1.0 / 255;
            var r = toDouble * color.R;
            var g = toDouble * color.G;
            var b = toDouble * color.B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

            if(chroma == 0)
            {
                h1 = 0;
            }
            else if(max == r)
            {
                h1 = (g - b) / chroma % 6;
            }
            else if(max == g)
            {
                h1 = 2 + (b - r) / chroma;
            }
            else //if (max == b)
            {
                h1 = 4 + (r - g) / chroma;
            }

            var lightness = 0.5 * (max - min);
            var saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs(2 * lightness - 1));
            return new(toDouble * color.A, 60 * h1, saturation, lightness);
        }

        public static string ToHexString(this Color color)
            => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        public static string ToHexAlphaString(this Color color)
            => $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Defines a color in Hue/Saturation/Lightness (HSL) space.
    /// </summary>
    /// <param name="A">The Alpha/opacity in 0..1 range.</param>
    /// <param name="H">The Hue in 0..360 range.</param>
    /// <param name="S">The Lightness in 0..1 range.</param>
    /// <param name="L">The Saturation in 0..1 range.</param>
    public readonly record struct HslColor(double A, double H, double S, double L);
}
