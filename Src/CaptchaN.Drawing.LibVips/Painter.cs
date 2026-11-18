using CaptchaN.Abstractions;
using NetVips;
using static NetVips.Enums;

namespace CaptchaN.Drawing.LibVips;

public sealed partial class Painter(PainterOption option) : IPainter
{
    public byte[] GenerateImage(string codeText, ImageSize size, PaintConfig config)
    {
        return CreateImage(codeText, size, config);
    }

    public string GenerateImageBase64Text(string codeText, ImageSize size, PaintConfig config)
    {
        var bytes = CreateImage(codeText, size, config);
        const string contentType = "image/jpeg";
        return $"data:{contentType};base64,{Convert.ToBase64String(bytes)}";
    }

    private byte[] CreateImage(string codeText, ImageSize size, PaintConfig config)
    {
        var random = Random.Shared;
        var bgColor = config.UseBlackWhiteOnly ? Colors.White : Colors.RandomLight(random);
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"[CaptchaN][Painter] Background Color: {bgColor}");
#endif
        using var pen = new MaoPen(CreateCanvas(
            CaptchaConstants.DefaultWidth,
            CaptchaConstants.DefaultHeight,
            bgColor))
        {
            CodeText = codeText,
            Size = size,
            PaintConfig = config,
            PainterOption = option,
            BackgroundColor = bgColor,
            Random = random
        };
        using var image = pen
            .DrawCode()
            .DrawPoints()
            .DrawBubbles()
            .DrawStars()
            .DrawLines()
            .DrawInterferChars()
            .Resize();
        return image.JpegsaveBuffer(q: option.Quality);
    }

    private static Image CreateCanvas(int width, int height, VipsColor color)
    {
        using var black = Image.Black(width, height, bands: 4);
        using var colored = black.NewFromImage(color.ToDoubleArray());
        return colored.Copy(interpretation: Interpretation.Srgb);
    }
}
