using System.Diagnostics.CodeAnalysis;
using ImageMagick;

namespace CaptchaN.Drawing.ImageMagick;

public static class Fonts
{
    public static IReadOnlyList<string> FontFamilies { get; set; } = [];

    public static string RandomPick(Random rand)
    {
        var families = FontFamilies;
        EnsureFontsLoaded(families);
        var family = families[rand.Next(families.Count)];
        return family;
    }

    public static void UseSystemFonts(Func<string, bool>? filter = default)
    {
        var families = filter is null
            ? MagickNET.FontFamilies
            : [.. MagickNET.FontFamilies.Where(filter)];
        EnsureFontsLoaded(families);
        FontFamilies = families;
    }

    public static void UseDirectoryFonts(DirectoryInfo directory)
    {
        if (directory.Exists)
        {
            var files = directory.GetFiles("*.ttf", SearchOption.TopDirectoryOnly);
            if (files is { Length: > 0 })
            {
                List<string> families = new(files.Length);
                foreach (var file in files)
                {
                    families.Add(file.FullName);
                }
                EnsureFontsLoaded(families);
                FontFamilies = families;
                return;
            }
        }
        Throw();
    }

    private static void EnsureFontsLoaded(IReadOnlyList<string> families)
    {
        if (families.Count == 0)
        {
            Throw();
        }
    }

    [DoesNotReturn]
    private static void Throw() => throw new InvalidOperationException("No fonts loaded!");
}
