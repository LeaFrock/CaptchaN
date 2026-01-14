using System.ComponentModel.DataAnnotations;

namespace CaptchaN.Drawing.ImageMagick;

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
                yield return new($"Font directory not found \n");
            }
        }
        if (!ValidateRange(FontOffsetXRange))
        {
            yield return new($"{nameof(FontOffsetXRange)} error \n");
        }
        if (!ValidateRange(FontOffsetYRange))
        {
            yield return new($"{nameof(FontOffsetYRange)} error \n");
        }
        if (Quality < 50 || Quality > 100)
        {
            yield return new($"{nameof(Quality)} must be between 50~100 \n");
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