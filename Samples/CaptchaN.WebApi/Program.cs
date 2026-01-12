using CaptchaN.Abstractions;
using CaptchaN.Drawing.ImageSharp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCaptchaN()
    .AddDefaultCodeTextGenerator()
    .AddPaintConfig(builder.Configuration.GetSection(nameof(PaintConfig)))
    .AddImageSharpPainter(builder.Configuration.GetSection("ImageSharpPainter"));

Fonts.UseDirectoryFonts(new(Path.Combine(builder.Environment.ContentRootPath, "Fonts")));

var app = builder.Build();

var painter = app.Services.GetRequiredService<IPainter>();
painter.NoOp(); // Make sure the painter is initialized correctly.

app.MapGet("/captcha", FileContentHttpResult (
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
});

app.Run();