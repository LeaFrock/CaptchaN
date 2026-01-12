namespace CaptchaN.Abstractions;

public readonly record struct ImageSize
{
    public ImageSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }
}