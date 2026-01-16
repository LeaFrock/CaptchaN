using CaptchaN.Abstractions;
using SkiaSharp;

namespace CaptchaN.Drawing.SkiaSharp;

public sealed partial class Painter(PainterOption option) : IPainter
{
    public byte[] GenerateImage(string codeText, ImageSize size, PaintConfig config)
    {
        using var imgData = CreateImage(codeText, size, config);
        return imgData.ToArray();
    }

    public string GenerateImageBase64Text(string codeText, ImageSize size, PaintConfig config)
    {
        using var imgData = CreateImage(codeText, size, config);
        const string contentType = "image/jpeg";
        return $"data:{contentType};base64,{Convert.ToBase64String(imgData.AsSpan())}";
    }

    private SKData CreateImage(string codeText, ImageSize size, PaintConfig config)
    {
        using var srcBitmap = new SKBitmap(CaptchaConstants.DefaultWidth, CaptchaConstants.DefaultHeight);
        using var bitmap = new MaoPen(srcBitmap)
        {
            CodeText = codeText,
            PaintConfig = config,
            PainterOption = option,
            Size = size,
            Random = Random.Shared
        }
        .Background()
        .DrawCode()
        .DrawPoints()
        .DrawBubbles()
        .DrawStars()
        .DrawLines()
        .DrawInterferChars()
        .Resize();
        using var img = SKImage.FromBitmap(bitmap);
        return img.Encode(SKEncodedImageFormat.Jpeg, option.Quality);
    }
}
