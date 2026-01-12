using System.ComponentModel.DataAnnotations;

namespace CaptchaN.Drawing.ImageSharp;

public sealed class PainterOption : IValidatableObject
{
    public string? FontFolderPath { get; set; }

    public int MaxFontSize { get; set; } = 50;

    /// <summary>
    /// Empirical range of random offset allowed along the font’s X-axis.
    /// </summary>
    public int[] FontOffsetXRange { get; set; } = [];

    /// <summary>
    /// Empirical range of random offset allowed along the font’s Y-axis.
    /// </summary>
    public int[] FontOffsetYRange { get; set; } = [];

    /// <summary>
    /// Image Encoding Quality (50-100).
    /// </summary>
    public int Quality { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(FontFolderPath))
        {
            if (!Directory.Exists(FontFolderPath))
            {
                yield return new($"字体文件夹不存在 \n");
            }
        }
        if (!ValidateRange(FontOffsetXRange))
        {
            yield return new($"{nameof(FontOffsetXRange)}配置错误 \n");
        }
        if (!ValidateRange(FontOffsetYRange))
        {
            yield return new($"{nameof(FontOffsetYRange)}配置错误 \n");
        }
        if (Quality < 50 || Quality > 100)
        {
            yield return new($"{nameof(Quality)}必须在50~100之间 \n");
        }
        yield return ValidationResult.Success!;
    }

    private static bool ValidateRange(int[] range)
    {
        if (range is not { Length: 2 })
        {
            return false;
        }
        return range[1] >= range[0];
    }
}