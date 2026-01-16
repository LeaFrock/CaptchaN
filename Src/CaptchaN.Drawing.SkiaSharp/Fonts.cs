using System.Diagnostics.CodeAnalysis;
using SkiaSharp;

namespace CaptchaN.Drawing.SkiaSharp;

public static class Fonts
{
    public static IReadOnlyList<SKTypeface> Typefaces { get; set; } = [];

    public static SKFont RandomPick(Random rand, float size)
    {
        var typefaces = Typefaces;
        EnsureFontsLoaded(typefaces);
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
        EnsureFontsLoaded(typefaces);
        Typefaces = typefaces;
    }

    public static void UseDirectoryFonts(DirectoryInfo directory)
    {
        if (directory.Exists)
        {
            List<SKTypeface> typefaces = [];
            var files = directory.GetFiles("*.ttf", SearchOption.TopDirectoryOnly);
            if (files is { Length: > 0 })
            {
                foreach (var file in files)
                {
                    using var s = file.OpenRead();
                    typefaces.Add(SKTypeface.FromStream(s));
                }
                EnsureFontsLoaded(typefaces);
                Typefaces = typefaces;
                return;
            }
        }
        Throw();
    }

    private static void EnsureFontsLoaded(IReadOnlyList<SKTypeface> typefaces)
    {
        if (typefaces.Count == 0)
        {
            Throw();
        }
    }

    [DoesNotReturn]
    private static void Throw() => throw new InvalidOperationException("No fonts loaded!");
}
