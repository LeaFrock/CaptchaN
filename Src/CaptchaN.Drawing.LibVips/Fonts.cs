using CaptchaN.Abstractions;

namespace CaptchaN.Drawing.LibVips;

public static class Fonts
{
    public static IReadOnlyList<VipsFont> FontFamilies { get; set; } = [];

    public static VipsFont RandomPick(Random rand)
    {
        var families = FontFamilies;
        if (families.Count == 0)
        {
            FontsNotLoadException.Throw();
        }

        return families[rand.Next(families.Count)];
    }

    public static void UseDirectoryFonts(DirectoryInfo directory)
    {
        if (!directory.Exists)
        {
            FontsNotLoadException.Throw(directory.FullName);
            return;
        }

        List<VipsFont> families = [];
        foreach (var file in directory.EnumerateFiles())
        {
            if (FontConstants.SingleFontFileSupported(file) || FontConstants.MultiFontFileSupported(file))
            {
                families.Add(VipsFont.FromFile(file.FullName));
            }
        }

        if (families.Count == 0)
        {
            FontsNotLoadException.Throw(directory.FullName);
            return;
        }

        FontFamilies = families;
    }
}

public readonly record struct VipsFont(string Family, string? FilePath = null)
{
    public static VipsFont FromFile(string filePath) => new("sans", filePath);

    public static implicit operator VipsFont(string family) => new(family);
}
