# CaptchaN

![Image Demo](https://raw.githubusercontent.com/LeaFrock/CaptchaN/main/Images/name.jpeg)

Generate captcha images based on dotNET. 
>用于生成图形验证码的开源.NET库。

[![.NET 10](https://img.shields.io/badge/.NET-10-brightgreen)](https://dotnet.microsoft.com/download)
![License](https://img.shields.io/badge/License-MIT-green)

## Packages |Nuget包

|             Name             |                           Description                            |                                                                          NugetPackage                                                                           |
| :--------------------------: | :--------------------------------------------------------------: | :-------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|    CaptchaN.Abstractions     |                       Abstractions module                        |           [![CaptchaN.Abstractions](https://img.shields.io/nuget/v/CaptchaN.Abstractions.svg)](https://www.nuget.org/packages/CaptchaN.Abstractions)            |
|  CaptchaN.Drawing.SkiaSharp  |    Powered by [SkiaSharp](https://github.com/mono/SkiaSharp)     |    [![CaptchaN.Drawing.SkiaSharp](https://img.shields.io/nuget/v/CaptchaN.Drawing.SkiaSharp.svg)](https://www.nuget.org/packages/CaptchaN.Drawing.SkiaSharp)    |
| CaptchaN.Drawing.ImageSharp  | Powered by [ImageSharp](https://github.com/SixLabors/ImageSharp) |  [![CaptchaN.Drawing.ImageSharp](https://img.shields.io/nuget/v/CaptchaN.Drawing.ImageSharp.svg)](https://www.nuget.org/packages/CaptchaN.Drawing.ImageSharp)   |
| CaptchaN.Drawing.ImageMagick | Powered by [Magick.NET](https://github.com/dlemstra/Magick.NET)  | [![CaptchaN.Drawing.ImageMagick](https://img.shields.io/nuget/v/CaptchaN.Drawing.ImageMagick.svg)](https://www.nuget.org/packages/CaptchaN.Drawing.ImageMagick) |

## QuickStart |快速入门

### Choose an underlying drawing engine you like | 选择一个你想用的绘图引擎

```shell
dotnet add package CaptchaN.Drawing.SkiaSharp

dotnet add package CaptchaN.Drawing.ImageSharp

dotnet add package CaptchaN.Drawing.ImageMagick
```

### Wiki & samples | 维基和代码示例

```csharp
using CaptchaN.Abstractions;
using CaptchaN.Drawing.SkiaSharp;

builder.Services.AddCaptchaN()
    .AddDefaultCodeTextGenerator()
    .AddPaintConfig(builder.Configuration.GetSection(nameof(PaintConfig)))
    .AddSkiaSharpPainter(builder.Configuration.GetSection("SkiaSharpPainter"));

// Tell CaptchaN where to load & use the font files
Fonts.UseDirectoryFonts(new(Path.Combine(builder.Environment.ContentRootPath, "Fonts")));
```

Please see [Wiki](https://github.com/LeaFrock/CaptchaN/wiki) and
[Samples](https://github.com/LeaFrock/CaptchaN/tree/main/Samples) for details.

## Contribution |贡献

Issues and pull requests are welcomed if you have any questions!
>如果您有任何疑问，欢迎提交Issue和PR！
