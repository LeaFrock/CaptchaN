using CaptchaN.Abstractions;
using SkiaSharp;

namespace CaptchaN.Drawing.SkiaSharp;

public static class Fonts
{
    public static IReadOnlyList<SKTypeface> Typefaces { get; set; } = [];

    public static SKFont RandomPick(Random rand, float size)
    {
        var typefaces = Typefaces;
        if (typefaces.Count == 0)
        {
            FontsNotLoadException.Throw();
        }

        var family = typefaces[rand.Next(typefaces.Count)];
        return family.ToFont(size);
    }

    public static void UseSystemFonts(Func<string, bool>? filter = default)
    {
        var familyNames = filter is null
            ? SKFontManager.Default.FontFamilies
            : SKFontManager.Default.FontFamilies.Where(filter);
        List<SKTypeface> typefaces = [];
        foreach (var fn in familyNames)
        {
            typefaces.Add(SKTypeface.FromFamilyName(fn));
        }
        if (typefaces.Count == 0)
        {
            FontsNotLoadException.Throw();
            return;
        }

        Typefaces = typefaces;
    }

    public static void UseDirectoryFonts(DirectoryInfo directory)
    {
        if (!directory.Exists)
        {
            FontsNotLoadException.Throw(directory.FullName);
            return;
        }

        List<SKTypeface> typefaces = [];
        foreach (var file in directory.EnumerateFiles())
        {
            if (FontConstants.SingleFontFileSupported(file) || FontConstants.MultiFontFileSupported(file))
            {
                typefaces.Add(SKTypeface.FromFile(file.FullName));
            }
        }

        if (typefaces.Count == 0)
        {
            FontsNotLoadException.Throw(directory.FullName);
            return;
        }

        Typefaces = typefaces;
    }
}
