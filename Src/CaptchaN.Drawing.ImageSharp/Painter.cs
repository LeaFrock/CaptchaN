using CaptchaN.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CaptchaN.Drawing.ImageSharp;

public sealed partial class Painter(PainterOption option) : IPainter
{
    private readonly JpegEncoder DafaultJpegEncoder = new() { Quality = option.Quality };

    public byte[] GenerateImage(string codeText, ImageSize size, PaintConfig config)
    {
        using Image<Rgba32> img = CreateImage(codeText, size, config);
        using var ms = new MemoryStream(4096);
        img.SaveAsJpeg(ms, DafaultJpegEncoder);
        return ms.ToArray();
    }

    public string GenerateImageBase64Text(string codeText, ImageSize size, PaintConfig config)
    {
        using Image<Rgba32> img = CreateImage(codeText, size, config);
        using var ms = new MemoryStream(4096);
        img.SaveAsJpeg(ms, DafaultJpegEncoder);

        var buffer = ms.GetBuffer();
        var format = JpegFormat.Instance;
        var base64Text = Convert.ToBase64String(buffer.AsSpan(0, (int)ms.Length));
        return $"data:{format.DefaultMimeType};base64,{base64Text}";
    }

    private Image<Rgba32> CreateImage(string codeText, ImageSize size, PaintConfig config)
    {
        Image<Rgba32> img = new(CaptchaConstants.DefaultWidth, CaptchaConstants.DefaultHeight);
        // img.Configuration.ImageFormatsManager.SetEncoder(JpegFormat.Instance, DafaultJpegEncoder);
        var penConf = new MaoPenConfig()
        {
            CodeText = codeText,
            PaintConfig = config,
            PainterOption = option,
            Size = size
        };
        img.Mutate(ctx =>
        {
            new MaoPen(ctx, Random.Shared, penConf)
                .Background()
                .DrawCode()
                .DrawPoints()
                .DrawBubbles()
                .DrawStars()
                .DrawLines()
                .DrawInterferChars()
                .Resize();
        });
        return img;
    }
}
