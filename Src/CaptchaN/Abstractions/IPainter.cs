namespace CaptchaN.Abstractions
{
    public interface IPainter
    {
        Task<string> GenerateImageBase64Async(string codeText, PainterOption option);

        Task<byte[]> GenerateImageAsync(string codeText, PainterOption option);
    }
}