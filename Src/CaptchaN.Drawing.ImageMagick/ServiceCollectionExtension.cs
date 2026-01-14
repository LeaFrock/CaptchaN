using CaptchaN.Abstractions;
using CaptchaN.Drawing.ImageMagick;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static void AddImageMagickPainter(this CaptchaNBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddOptions<PainterOption>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddPainter(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<PainterOption>>().Value;
            if (!string.IsNullOrEmpty(opt.FontFolderPath))
            {
                // Fonts.UseDirectoryFonts(new(opt.FontFolderPath));
            }
            return new Painter(opt);
        });
    }
}
