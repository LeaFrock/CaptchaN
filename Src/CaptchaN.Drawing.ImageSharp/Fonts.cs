using CaptchaN.Abstractions;
using SixLabors.Fonts;

namespace CaptchaN.Drawing.ImageSharp;

public static class Fonts
{
    public static IReadOnlyList<FontFamily> FontFamilies { get; set; } = [];

    public static Font RandomPick(Random rand, float size, FontStyle style = FontStyle.Regular)
    {
        var families = FontFamilies;
        if (families.Count == 0)
        {
            FontsNotLoadException.Throw();
        }

        var family = families[rand.Next(families.Count)];
        return family.CreateFont(size, style);
    }

    public static void UseSystemFonts(Predicate<FontMetrics>? match)
    {
        var fc = new FontCollection();
        if (match is null)
        {
            fc.AddSystemFonts();
        }
        else
        {
            fc.AddSystemFonts(match);
        }
        var families = fc.Families.ToArray();
        if (families.Length == 0)
        {
            FontsNotLoadException.Throw(SystemFonts.Collection.SearchDirectories);
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

        var fc = new FontCollection();
        foreach (var file in directory.EnumerateFiles())
        {
            if (FontConstants.SingleFontFileSupported(file))
            {
                fc.Add(file.FullName);
            }
            else if (FontConstants.MultiFontFileSupported(file))
            {
                fc.AddCollection(file.FullName);
            }
        }
        var families = fc.Families.ToArray();
        if (families.Length == 0)
        {
            FontsNotLoadException.Throw(directory.FullName);
            return;
        }

        FontFamilies = families;
    }
}
