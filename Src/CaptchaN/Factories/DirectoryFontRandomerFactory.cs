using System.IO;
using CaptchaN.Abstractions;
using SixLabors.Fonts;

namespace CaptchaN.Factories
{
    public class DirectoryFontRandomerFactory : IFontRandomerFactory
    {
        public DirectoryInfo FontDir { get; init; }

        public IFontRandomer CreateFontRandomer()
        {
            var fc = new FontCollection();
            var files = FontDir.GetFiles("*.ttf", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                using var s = file.OpenRead();
                fc.Install(s);
            }
            return new FontRandomer(fc);
        }
    }
}