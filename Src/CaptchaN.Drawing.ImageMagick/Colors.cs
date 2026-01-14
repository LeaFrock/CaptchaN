using System.Collections.Concurrent;
using ImageMagick;

namespace CaptchaN.Drawing.ImageMagick;

public static class Colors
{
    private static readonly ConcurrentDictionary<string, MagickColor[]> AlphaColorsCache = new();

    public static IReadOnlyList<MagickColorRef> DarkPool { get; set; } = [
            MagickColors.DarkBlue,
            MagickColors.DarkCyan,
            MagickColors.DarkGoldenrod,
            MagickColors.DarkGreen,
            MagickColors.DarkKhaki,
            MagickColors.DarkMagenta,
            MagickColors.DarkOliveGreen,
            MagickColors.DarkOrange,
            MagickColors.DarkOrchid,
            MagickColors.DarkRed,
            MagickColors.DarkSalmon,
            MagickColors.DarkSeaGreen,
            MagickColors.DarkSlateBlue,
            MagickColors.DarkSlateGray,
            MagickColors.DarkTurquoise,
            MagickColors.DarkViolet,
            MagickColors.DeepPink,
            MagickColors.DeepSkyBlue,
            MagickColors.DimGray,
            MagickColors.DodgerBlue,
            MagickColors.Firebrick,
            MagickColors.ForestGreen,
            MagickColors.Indigo,
            MagickColors.Maroon,
            MagickColors.MediumBlue,
            MagickColors.MediumVioletRed,
            MagickColors.MidnightBlue,
            MagickColors.Navy,
            MagickColors.Olive,
            MagickColors.OliveDrab,
            MagickColors.OrangeRed,
            MagickColors.Purple,
            MagickColors.RebeccaPurple,
            MagickColors.Red,
            MagickColors.RoyalBlue,
            MagickColors.SaddleBrown,
            MagickColors.SeaGreen,
            MagickColors.Sienna,
            MagickColors.SlateBlue,
            MagickColors.SlateGray,
            MagickColors.SteelBlue,
            MagickColors.Teal,
            MagickColors.Tomato
        ];

    public static IReadOnlyList<MagickColorRef> LightPool { get; set; } = [
            MagickColors.AliceBlue,
            MagickColors.AntiqueWhite,
            MagickColors.Azure,
            MagickColors.Beige,
            MagickColors.Bisque,
            MagickColors.BlanchedAlmond,
            MagickColors.Cornsilk,
            MagickColors.FloralWhite,
            MagickColors.Gainsboro,
            MagickColors.GhostWhite,
            MagickColors.Honeydew,
            MagickColors.Ivory,
            MagickColors.Lavender,
            MagickColors.LavenderBlush,
            MagickColors.LemonChiffon,
            MagickColors.LightBlue,
            MagickColors.LightCoral,
            MagickColors.LightCyan,
            MagickColors.LightGoldenrodYellow,
            MagickColors.LightGray,
            MagickColors.LightGreen,
            MagickColors.LightPink,
            MagickColors.LightSalmon,
            MagickColors.LightSeaGreen,
            MagickColors.LightSkyBlue,
            MagickColors.LightSteelBlue,
            MagickColors.LightYellow,
            MagickColors.Linen,
            MagickColors.MintCream,
            MagickColors.MistyRose,
            MagickColors.Moccasin,
            MagickColors.NavajoWhite,
            MagickColors.OldLace,
            MagickColors.PaleGoldenrod,
            MagickColors.PaleGreen,
            MagickColors.PaleTurquoise,
            MagickColors.PaleVioletRed,
            MagickColors.PapayaWhip,
            MagickColors.PeachPuff,
            MagickColors.Pink,
            MagickColors.Plum,
            MagickColors.PowderBlue,
            MagickColors.SeaShell,
            MagickColors.Silver,
            MagickColors.SkyBlue,
            MagickColors.Snow,
            MagickColors.Thistle,
            MagickColors.Wheat,
            MagickColors.WhiteSmoke
        ];

    public static MagickColorRef RandomDark(Random rand) => DarkPool[rand.Next(DarkPool.Count)];

    public static MagickColor RandomDark(Random rand, byte alpha) => RandomDark(rand).WithAlpha(alpha);

    public static MagickColorRef RandomLight(Random rand) => LightPool[rand.Next(LightPool.Count)];

    public static MagickColor RandomLight(Random rand, byte alpha) => RandomLight(rand).WithAlpha(alpha);

    public static MagickColor WithAlpha(this MagickColorRef color, byte a)
    {
        ArgumentOutOfRangeException.ThrowIfZero(a);

        if (((MagickColor)color).A != byte.MaxValue)
        {
            return new(color) { A = a };
        }
        var cache = AlphaColorsCache.GetOrAdd(color, static _ => new MagickColor[254]);
        if (cache[a - 1] is null)
        {
            cache[a - 1] = new((MagickColor)color) { A = a };
        }
        return cache[a - 1];
    }
}

public readonly struct MagickColorRef(MagickColor color) : IEquatable<MagickColorRef>
{
    private readonly MagickColor _color = color;

    public string Hex { get; } = color.ToHexString();

    public bool Equals(MagickColorRef other) => Hex.Equals(other.Hex, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is MagickColorRef @ref && Equals(@ref);

    public override int GetHashCode() => Hex.GetHashCode();

    public static implicit operator MagickColor(MagickColorRef @ref) => @ref._color;

    public static implicit operator string(MagickColorRef @ref) => @ref.Hex;

    public static implicit operator MagickColorRef(MagickColor c) => new(c);

    public static bool operator ==(MagickColorRef left, MagickColorRef right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MagickColorRef left, MagickColorRef right)
    {
        return !(left == right);
    }
}