using CaptchaN.Abstractions;
using ImageMagick;
using ImageMagick.Drawing;

namespace CaptchaN.Drawing.ImageMagick;

public sealed partial class Painter(PainterOption option) : IPainter
{
    // https://github.com/dlemstra/Magick.NET/blob/main/docs/Drawing.md

    public byte[] GenerateImage(string codeText, ImageSize size, PaintConfig config)
    {
        using var image = CreateImage(codeText, size, config);
        return image.ToByteArray();
    }

    public string GenerateImageBase64Text(string codeText, ImageSize size, PaintConfig config)
    {
        using var image = CreateImage(codeText, size, config);
        const string contentType = "image/jpeg";
        return $"data:{contentType};base64,{image.ToBase64()}";
    }

    private MagickImage CreateImage(string codeText, ImageSize size, PaintConfig config)
    {
        var random = Random.Shared;
        // var bgColor = MagickColors.LightSteelBlue;
        MagickColor bgColor = config.UseBlackWhiteOnly ? MagickColors.White : Colors.RandomLight(random);
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"[CaptchaN][Painter] Background Color: {bgColor}");
#endif
        var image = new MagickImage(bgColor, CaptchaConstants.DefaultWidth, CaptchaConstants.DefaultHeight)
        {
            Format = MagickFormat.Jpeg,
            Quality = (uint)option.Quality,
        };

        new MaoPen(image, new Drawables())
        {
            CodeText = codeText,
            Size = size,
            PaintConfig = config,
            PainterOption = option,
            Random = random
        }
        .DrawCode()
        .DrawPoints()
        .DrawBubbles()
        .DrawStars()
        .DrawLines()
        .DrawInterferChars()
        .Resize();

        return image;
    }
}