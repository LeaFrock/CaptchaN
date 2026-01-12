namespace CaptchaN.Abstractions;

public interface IPainter
{
    string GenerateImageBase64Text(string codeText, ImageSize size, PaintConfig config);

    byte[] GenerateImage(string codeText, ImageSize size, PaintConfig config);

    public void NoOp() { }
}