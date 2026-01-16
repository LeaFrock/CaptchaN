using SkiaSharp;

namespace CaptchaN.Drawing.SkiaSharp;

public static class Colors
{
    public static IReadOnlyList<SKColor> DarkPool { get; set; } =
        [
            SKColors.Black,
            SKColors.MidnightBlue,
            SKColors.MediumBlue,
            SKColors.DarkGreen,
            SKColors.DarkOliveGreen,
            SKColors.DarkGoldenrod,
            SKColors.SaddleBrown,
            SKColors.Brown,
            SKColors.OrangeRed,
            SKColors.Maroon,
            SKColors.DarkViolet
        ];

    public static IReadOnlyList<SKColor> LightPool { get; set; } =
        [
            SKColors.White,
            SKColors.Snow,
            SKColors.GhostWhite,
            SKColors.WhiteSmoke,
            SKColors.Gainsboro,
            SKColors.FloralWhite,
            SKColors.OldLace,
            SKColors.Linen,
            SKColors.Cornsilk,
            SKColors.Ivory,
            SKColors.LemonChiffon,
            SKColors.SeaShell,
            SKColors.Honeydew,
            SKColors.MintCream,
            SKColors.Azure,
            SKColors.AliceBlue,
            SKColors.Lavender,
            SKColors.LavenderBlush,
            SKColors.MistyRose,
            SKColors.LightGray,
            SKColors.LightSkyBlue,
            SKColors.LightYellow,
            SKColors.Pink,
            SKColors.PaleGreen
        ];

    public static SKColor RandomDark(Random rand) => DarkPool[rand.Next(DarkPool.Count)];

    public static SKColor RandomDark(Random rand, byte alpha) => RandomDark(rand).WithAlpha(alpha);

    public static SKColor RandomLight(Random rand) => LightPool[rand.Next(LightPool.Count)];

    public static SKColor RandomLight(Random rand, byte alpha) => RandomLight(rand).WithAlpha(alpha);
}
