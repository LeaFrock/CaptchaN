# CaptchaN

[DotNetUrl]: https://dotnet.microsoft.com/download
[CaptchaN-Abstractions-SvgUrl]: https://img.shields.io/nuget/v/CaptchaN.Abstractions.svg
[CaptchaN-Abstractions-NugetUrl]: https://www.nuget.org/packages/CaptchaN.Abstractions
[CaptchaN-Drawing-ImageSharp-SvgUrl]: https://img.shields.io/nuget/v/CaptchaN.Drawing.ImageSharp.svg
[CaptchaN-Drawing-ImageSharp-NugetUrl]: https://www.nuget.org/packages/CaptchaN.Drawing.ImageSharp

![Image Demo](https://raw.githubusercontent.com/LeaFrock/CaptchaN/main/Images/name.jpeg)

Generate captcha images based on dotNET.
>用于生成图形验证码的开源.NET库。

[![.NET 10](https://img.shields.io/badge/.NET-10-brightgreen)][DotNetUrl]
![License](https://img.shields.io/badge/License-MIT-green)

## Packages |Nuget包

|         Name          |           Description           |                                       NugetPackage                                       |
| :-------------------: | :-----------------------------: | :--------------------------------------------------------------------------------------: |
|       CaptchaN.Abstractions       |           Abstractions module           |                    [![CaptchaN.Abstractions][CaptchaN-Abstractions-SvgUrl]][CaptchaN-Abstractions-NugetUrl]                     |
| CaptchaN.Drawing.ImageSharp | Powered by [ImageSharp](https://github.com/SixLabors/ImageSharp) | [![CaptchaN.Drawing.ImageSharp][CaptchaN-Drawing-ImageSharp-SvgUrl]][CaptchaN-Drawing-ImageSharp-NugetUrl] |

## QuickStart |快速入门

```csharp
using CaptchaN.Abstractions;
using CaptchaN.Drawing.ImageSharp;

builder.Services.AddCaptchaN()
    .AddDefaultCodeTextGenerator()
    .AddPaintConfig(builder.Configuration.GetSection(nameof(PaintConfig)))
    .AddImageSharpPainter(builder.Configuration.GetSection("ImageSharpPainter"));

Fonts.UseDirectoryFonts(new(Path.Combine(builder.Environment.ContentRootPath, "Fonts")));
```

Please see [Wiki](https://github.com/LeaFrock/CaptchaN/wiki) and [Samples](https://github.com/LeaFrock/CaptchaN/tree/main/Samples) for details.

## Contribution |贡献

Issues and pull requests are welcomed if you have any questions!
>如果您有任何疑问，欢迎提交Issue和PR！
