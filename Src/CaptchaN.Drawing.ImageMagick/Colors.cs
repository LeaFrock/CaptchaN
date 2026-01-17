using System.Collections.Concurrent;
using ImageMagick;

namespace CaptchaN.Drawing.ImageMagick;

public static class Colors
{
    private static readonly ConcurrentDictionary<string, MagickColor[]> AlphaColorsCache = new();

    public static IReadOnlyList<MagickColorRef> DarkPool { get; set; } = [
            MagickColors.Black,
            MagickColors.MidnightBlue,
            MagickColors.MediumBlue,
            MagickColors.DarkGreen,
            MagickColors.DarkOliveGreen,
            MagickColors.DarkGoldenrod,
            MagickColors.SaddleBrown,
            MagickColors.Brown,
            MagickColors.OrangeRed,
            MagickColors.Maroon,
            MagickColors.DarkViolet
        ];

    public static IReadOnlyList<MagickColorRef> LightPool { get; set; } = [
            MagickColors.White,
            MagickColors.Snow,
            MagickColors.GhostWhite,
            MagickColors.WhiteSmoke,
            MagickColors.Gainsboro,
            MagickColors.FloralWhite,
            MagickColors.OldLace,
            MagickColors.Linen,
            MagickColors.Cornsilk,
            MagickColors.Ivory,
            MagickColors.LemonChiffon,
            MagickColors.SeaShell,
            MagickColors.Honeydew,
            MagickColors.MintCream,
            MagickColors.Azure,
            MagickColors.AliceBlue,
            MagickColors.Lavender,
            MagickColors.LavenderBlush,
            MagickColors.MistyRose,
            MagickColors.LightGray,
            MagickColors.LightSkyBlue,
            MagickColors.LightYellow,
            MagickColors.Pink,
            MagickColors.PaleGreen
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
        var cache = AlphaColorsCache.GetOrAdd(color, static _ => new MagickColor[254]); // alpha from 1~254
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