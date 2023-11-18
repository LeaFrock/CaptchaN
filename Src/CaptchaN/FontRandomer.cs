using CaptchaN.Abstractions;
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
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfLessThan(fontFamilies.Length, 1, nameof(fontFamilies));
#else
            if (fontFamilies.Length < 1)
            {

                throw new ArgumentOutOfRangeException(nameof(fontFamilies), "Array length must be over 0.");
            }
#endif

            _fontFamilies = fontFamilies;
        }

        public Font Random(float size, FontStyle fontStyle)
        {
            int index = System.Random.Shared.Next(_fontFamilies.Length);
#if DEBUG
            System.Diagnostics.Debug.WriteLine(_fontFamilies[index].Name);
#endif
            return _fontFamilies[index].CreateFont(size, fontStyle);
        }
    }
}