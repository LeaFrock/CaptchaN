using CaptchaN.Abstractions;
using CaptchaN.Helpers;
using SixLabors.Fonts;

namespace CaptchaN
{
    public class FontRandomer : IFontRandomer
    {
        private readonly FontFamily[] _fontFamilies;

        internal FontRandomer(FontCollection fontCollection) : this(fontCollection.Families?.ToArray())
        {
        }

        internal FontRandomer(FontFamily[] fontFamilies)
        {
            ArgumentNullException.ThrowIfNull(fontFamilies);
            if(fontFamilies.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(fontFamilies), "Array length must be over 0.");
            }

            _fontFamilies = fontFamilies;
        }

        public Font Random(float size, FontStyle fontStyle)
        {
            int index = RandomHelper.CurrentRandom.Next(0, _fontFamilies.Length);
#if DEBUG
            System.Diagnostics.Debug.WriteLine(_fontFamilies[index].Name);
#endif
            return _fontFamilies[index].CreateFont(size, fontStyle);
        }
    }
}