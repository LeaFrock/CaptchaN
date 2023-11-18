using CaptchaN.Abstractions;
using CaptchaN.Factories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PainterOption>(builder.Configuration.GetSection("CaptchaN"));
builder.Services.AddCaptchaN()
                //.UseFontRandomer(new SystemFontRandomerFactory());
                .UseFontRandomer(new DirectoryFontRandomerFactory() { FontDir = new(Path.Combine(builder.Environment.ContentRootPath, "Fonts")) });

var app = builder.Build();

app.MapGet("/captcha", async Task<FileContentHttpResult> (IOptions<PainterOption> painterOpt, ICodeTextGenerator codeTextGenerator, IPainter painter) =>
{
    var codeText = codeTextGenerator.Generate(4);
    var image = await painter.GenerateImageAsync(codeText, painterOpt.Value);
    return TypedResults.File(image, "image/jpeg; charset=UTF-8");
});

app.Run();