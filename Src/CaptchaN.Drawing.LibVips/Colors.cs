namespace CaptchaN.Drawing.LibVips;

public static class Colors
{
    public static VipsColor Black => new(0, 0, 0);

    public static VipsColor White => new(255, 255, 255);

    public static IReadOnlyList<VipsColor> DarkPool { get; set; } =
        [
            Black,
            new(25, 25, 112),
            new(0, 0, 205),
            new(0, 100, 0),
            new(85, 107, 47),
            new(184, 134, 11),
            new(139, 69, 19),
            new(165, 42, 42),
            new(255, 69, 0),
            new(128, 0, 0),
            new(148, 0, 211)
        ];

    public static IReadOnlyList<VipsColor> LightPool { get; set; } =
        [
            White,
            new(255, 250, 250),
            new(248, 248, 255),
            new(245, 245, 245),
            new(220, 220, 220),
            new(255, 250, 240),
            new(253, 245, 230),
            new(250, 240, 230),
            new(255, 248, 220),
            new(255, 255, 240),
            new(255, 250, 205),
            new(255, 245, 238),
            new(240, 255, 240),
            new(245, 255, 250),
            new(240, 255, 255),
            new(240, 248, 255),
            new(230, 230, 250),
            new(255, 240, 245),
            new(255, 228, 225),
            new(211, 211, 211),
            new(135, 206, 250),
            new(255, 255, 224),
            new(255, 192, 203),
            new(152, 251, 152)
        ];

    public static VipsColor RandomDark(Random rand) => DarkPool[rand.Next(DarkPool.Count)];

    public static VipsColor RandomDark(Random rand, byte alpha) => RandomDark(rand).WithAlpha(alpha);

    public static VipsColor RandomLight(Random rand) => LightPool[rand.Next(LightPool.Count)];

    public static VipsColor RandomLight(Random rand, byte alpha) => RandomLight(rand).WithAlpha(alpha);
}

public readonly record struct VipsColor(byte R, byte G, byte B, byte A = byte.MaxValue)
{
    public VipsColor WithAlpha(byte alpha) => this with { A = alpha };

    public double[] ToDoubleArray() => [R, G, B, A];

    public double[] ToRgbArray() => [R, G, B];
}
