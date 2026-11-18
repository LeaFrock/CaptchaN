using CaptchaN.Abstractions;
using CaptchaN.Drawing.LibVips;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NetVips;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static void AddLibVipsPainter(this CaptchaNBuilder builder, IConfiguration configuration)
    {
        if (!ModuleInitializer.VipsInitialized)
        {
            throw ModuleInitializer.Exception;
        }

        builder.Services.AddOptions<PainterOption>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddPainter(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<PainterOption>>().Value;
            if (!string.IsNullOrEmpty(opt.FontFolderPath))
            {
                Fonts.UseDirectoryFonts(new(opt.FontFolderPath));
            }
            return new Painter(opt);
        });
    }
}
