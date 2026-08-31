namespace CaptchaN.Abstractions;

public static class FontConstants
{
    private static readonly HashSet<string> _supportedSingleFontFileExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".ttf",
        ".otf",
        // ".woff2"
    };

    private static readonly HashSet<string> _supportedMultiFontFileExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".ttc",
        ".otc",
    };

    public static bool SingleFontFileSupported(FileInfo file) => _supportedSingleFontFileExtensions.Contains(file.Extension);

    public static bool MultiFontFileSupported(FileInfo file) => _supportedMultiFontFileExtensions.Contains(file.Extension);
}
