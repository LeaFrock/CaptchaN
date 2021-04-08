using SixLabors.Fonts;

namespace CaptchaN.Abstractions
{
    public interface IFontRandomer
    {
        Font Random(float size, FontStyle fontStyle);
    }
}