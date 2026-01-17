using CaptchaN.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.painter.json", optional: false, reloadOnChange: true);

builder.Services.AddCaptchaN()
    .AddDefaultCodeTextGenerator()
    .AddPaintConfig(builder.Configuration.GetSection(nameof(PaintConfig)))
    .AddSkiaSharpPainter(builder.Configuration.GetSection("SkiaSharp"))
    //.AddImageSharpPainter(builder.Configuration.GetSection("ImageSharp"))
    //.AddImageMagickPainter(builder.Configuration.GetSection("ImageMagick"))
    ;

CaptchaN.Drawing.SkiaSharp.Fonts.UseDirectoryFonts(new(Path.Combine(builder.Environment.ContentRootPath, "Fonts")));
// CaptchaN.Drawing.ImageSharp.Fonts.UseDirectoryFonts(new(Path.Combine(builder.Environment.ContentRootPath, "Fonts")));
// CaptchaN.Drawing.ImageMagick.Fonts.UseDirectoryFonts(new(Path.Combine(builder.Environment.ContentRootPath, "Fonts")));

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole();

var app = builder.Build();

var painter = app.Services.GetRequiredService<IPainter>();
painter.NoOp(); // Make sure the painter is initialized correctly.

app.MapGet("/captcha", IResult (
    [FromQuery(Name = "sc")] int? sizeCode,
    [FromServices] ICodeTextGenerator codeTextGenerator,
    [FromServices] IPainter painter,
    [FromServices] IOptions<PaintConfig> configOpt) =>
{
    ImageSize size = sizeCode switch
    {
        1 => new(80, 20),
        2 => new(100, 25),
        3 => new(160, 40),
        4 => new(240, 60),
        // 5 => new(320, 80),
        _ => new(240, 60)
    };
    var codeText = codeTextGenerator.Generate(4);
    var image = painter.GenerateImage(codeText, size, configOpt.Value);
    return TypedResults.File(image, "image/jpeg; charset=UTF-8");
    //var imageBase64 = painter.GenerateImageBase64Text(codeText, size, configOpt.Value);
    //return TypedResults.Ok(imageBase64);
});

app.Run();