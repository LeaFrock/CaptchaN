using System.Diagnostics.CodeAnalysis;
using SixLabors.Fonts;

namespace CaptchaN.Drawing.ImageSharp;

public static class Fonts
{
    public static IReadOnlyList<FontFamily> FontFamilies { get; set; } = [];

    public static Font RandomPick(Random rand, float size, FontStyle style = FontStyle.Regular)
    {
        var families = FontFamilies;
        EnsureFontsLoaded(families);
        var family = families[rand.Next(families.Count)];
        return family.CreateFont(size, style);
    }

    public static void UseSystemFonts(Func<FontFamily, bool>? filter = default)
    {
        var fc = new FontCollection();
        fc.AddSystemFonts();
        var families = filter is null
            ? fc.Families.ToArray()
            : [.. fc.Families.Where(filter)];
        EnsureFontsLoaded(families);
        FontFamilies = families;
    }

    public static void UseDirectoryFonts(DirectoryInfo directory)
    {
        if (directory.Exists)
        {
            var fc = new FontCollection();
            var files = directory.GetFiles("*.ttf", SearchOption.TopDirectoryOnly);
            if (files is { Length: > 0 })
            {
                foreach (var file in files)
                {
                    using var s = file.OpenRead();
                    fc.Add(s);
                }
                var families = fc.Families.ToArray();
                EnsureFontsLoaded(families);
                FontFamilies = families;
                return;
            }
        }
        Throw();
    }

    private static void EnsureFontsLoaded(IReadOnlyList<FontFamily> families)
    {
        if (families.Count == 0)
        {
            Throw();
        }
    }

    [DoesNotReturn]
    private static void Throw() => throw new InvalidOperationException("No fonts loaded!");
}
