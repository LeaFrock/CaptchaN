using CaptchaN.Abstractions;
using ImageMagick;

namespace CaptchaN.Drawing.ImageMagick;

public static class Fonts
{
    public static IReadOnlyList<string> FontFamilies { get; set; } = [];

    public static string RandomPick(Random rand)
    {
        var families = FontFamilies;
        if (families.Count == 0)
        {
            FontsNotLoadException.Throw();
        }

        var family = families[rand.Next(families.Count)];
        return family;
    }

    public static void UseSystemFonts(Func<string, bool>? filter = default)
    {
        var families = filter is null
            ? MagickNET.FontFamilies
            : [.. MagickNET.FontFamilies.Where(filter)];
        if (families.Count == 0)
        {
            FontsNotLoadException.Throw();
            return;
        }
        FontFamilies = families;
    }

    public static void UseDirectoryFonts(DirectoryInfo directory)
    {
        if (!directory.Exists)
        {
            FontsNotLoadException.Throw(directory.FullName);
            return;
        }

        List<string> families = [];
        foreach (var file in directory.EnumerateFiles())
        {
            if (FontConstants.SingleFontFileSupported(file) || FontConstants.MultiFontFileSupported(file))
            {
                families.Add(file.FullName);
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
