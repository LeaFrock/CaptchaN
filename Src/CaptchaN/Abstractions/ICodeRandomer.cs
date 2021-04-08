using SixLabors.ImageSharp;

namespace CaptchaN.Abstractions
{
    public interface IColorRandomer
    {
        Color RandomDark();

        Color RandomLight();
    }
}