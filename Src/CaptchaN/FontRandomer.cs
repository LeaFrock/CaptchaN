using CaptchaN.Abstractions;
using CaptchaN.Helpers;
using SixLabors.Fonts;

namespace CaptchaN
{
    public class FontRandomer : IFontRandomer
    {
        private readonly FontFamily[] _fontFamilies;

        internal FontRandomer(FontCollection fontCollection)
        {
            _fontFamilies = fontCollection.Families.ToArray();
        }

        public Font Random(float size, FontStyle fontStyle)
        {
            int index = RandomHelper.CurrentRandom.Next(0, _fontFamilies.Length);
            return _fontFamilies[index].CreateFont(size, fontStyle);
        }
    }
}