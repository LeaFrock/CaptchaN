using CaptchaN.Abstractions;
using CaptchaN.Helpers;
using SixLabors.ImageSharp;

namespace CaptchaN
{
    public class ColorRandomer : IColorRandomer
    {
        private readonly static Color[] _darkPool = new Color[]
        {
            Color.Black,
            Color.MidnightBlue,
            Color.MediumBlue,
            Color.DarkGreen,
            Color.DarkOliveGreen,
            Color.DarkGoldenrod,
            Color.SaddleBrown,
            Color.Brown,
            Color.OrangeRed,
            Color.Maroon,
            Color.DarkViolet
        };

        private readonly static Color[] _lightPool = new Color[]
        {
            Color.White,
            Color.Snow,
            Color.GhostWhite,
            Color.WhiteSmoke,
            Color.Gainsboro,
            Color.FloralWhite,
            Color.OldLace,
            Color.Linen,
            Color.Cornsilk,
            Color.Ivory,
            Color.LemonChiffon,
            Color.SeaShell,
            Color.Honeydew,
            Color.MintCream,
            Color.Azure,
            Color.AliceBlue,
            Color.Lavender,
            Color.LavenderBlush,
            Color.MistyRose,
            Color.LightGray,
            Color.LightSkyBlue,
            Color.LightYellow,
            Color.Pink,
            Color.PaleGreen
        };

        public Color RandomDark() => RandomColor(_darkPool);

        public Color RandomLight() => RandomColor(_lightPool);

        private static Color RandomColor(Color[] pool)
        {
            int index = RandomHelper.CurrentRandom.Next(0, pool.Length);
            return pool[index];
        }
    }
}