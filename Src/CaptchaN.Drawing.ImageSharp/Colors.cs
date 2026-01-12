using SixLabors.ImageSharp;

namespace CaptchaN.Drawing.ImageSharp;

public static class Colors
{
    public static IReadOnlyList<Color> DarkPool { get; set; } =
        [
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
        ];

    public static IReadOnlyList<Color> LightPool { get; set; } =
        [
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
        ];

    public static Color RandomDark(Random rand) => DarkPool[rand.Next(DarkPool.Count)];

    public static Color RandomDark(Random rand, float alpha) => RandomDark(rand).WithAlpha(alpha);

    public static Color RandomLight(Random rand) => LightPool[rand.Next(LightPool.Count)];

    public static Color RandomLight(Random rand, float alpha) => RandomLight(rand).WithAlpha(alpha);
}
