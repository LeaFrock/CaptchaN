# CaptchaN

[DotNetUrl]: https://dotnet.microsoft.com/download
[ImageSharpUrl]: https://github.com/SixLabors/ImageSharp
[CaptchaN-SvgUrl]: https://img.shields.io/nuget/v/CaptchaN.svg
[CaptchaN-NugetUrl]: https://www.nuget.org/packages/CaptchaN
[CaptchaN-DI-Microsoft-SvgUrl]: https://img.shields.io/nuget/v/CaptchaN.DI.Microsoft.svg
[CaptchaN-DI-Microsoft-NugetUrl]: https://www.nuget.org/packages/CaptchaN.DI.Microsoft

![Image Demo](https://raw.githubusercontent.com/LeaFrock/CaptchaN/main/Images/name.jpeg)

Generate captcha images based on [ImageSharp][ImageSharpUrl] and .NET.
>基于[ImageSharp][ImageSharpUrl]项目和.NET，生成图形验证码。

[![.NET 8](https://img.shields.io/badge/.NET-8-brightgreen)][DotNetUrl]
![License](https://img.shields.io/badge/License-MIT-green)

## Packages |Nuget包

|         Name          |           Description           |                                       NugetPackage                                       |
| :-------------------: | :-----------------------------: | :--------------------------------------------------------------------------------------: |
|       CaptchaN        |           Core module           |                    [![CaptchaN][CaptchaN-SvgUrl]][CaptchaN-NugetUrl]                     |
| CaptchaN.DI.Microsoft | DI module for default container | [![CaptchaN.DI.Microsoft][CaptchaN-DI-Microsoft-SvgUrl]][CaptchaN-DI-Microsoft-NugetUrl] |

## QuickStart |快速入门

The following is main codes for basic usage.

``` C#
// Load or install fonts(*.ttf) which are provided by yourself
IFontRandomerFactory fontRandomerFactory = new ...;
IFontRandomer fontRandomer = fontRandomerFactory.CreateFontRandomer();
// Init color randomer
IColorRandomer colorRandomer = new ...;
IPainter painter = new Painter(fontRandomer, colorRandomer);
// Init the content settings for images
PainterOption painterOption = new(){...};
// Optional. Generate random text for solid length
ICodeTextGenerator codeTextGenerator = new ...;
string codeText = codeTextGenerator.Generate(4);
// Generate an image
await painter.GenerateImageAsync(codeText, painterOption);
```

Please see [Wiki](https://github.com/LeaFrock/CaptchaN/wiki) and [Samples](https://github.com/LeaFrock/CaptchaN/tree/main/Samples) for details.

## Contribution |贡献

Issues and pull requests are welcomed if you have any questions!
>如果您有任何疑问，欢迎提交Issue和PR！
