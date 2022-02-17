using CaptchaN.Abstractions;
using SixLabors.Fonts;

namespace CaptchaN.Factories
{
    public class SystemFontRandomerFactory : IFontRandomerFactory
    {
        public Func<FontFamily, bool> FontFamilyFilter { get; set; }

        public IFontRandomer CreateFontRandomer()
        {
            var fc = new FontCollection();
            fc.AddSystemFonts();
            var fs = FontFamilyFilter is null
                ? fc.Families.ToArray()
                : fc.Families.Where(FontFamilyFilter).ToArray();
            return new FontRandomer(fs);
        }
    }
}