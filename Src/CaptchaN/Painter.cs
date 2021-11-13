using CaptchaN.Abstractions;
using CaptchaN.Helpers;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CaptchaN
{
    public class Painter : IPainter
    {
        public const int DefaultPointSize = 2;
        public const int DefaultLineThickness = 1;

        private readonly IFontRandomer _fontRandomer;
        private readonly IColorRandomer _colorRandomer;

        public Painter(
            IFontRandomer fontRandomer,
            IColorRandomer colorRandomer)
        {
            _fontRandomer = fontRandomer;
            _colorRandomer = colorRandomer;
        }

        public async Task<byte[]> GenerateImageAsync(string codeText, PainterOption option)
        {
            using Image<Rgba32> img = CreateImage(codeText, option);
            using var ms = new MemoryStream();
            await img.SaveAsJpegAsync(ms);
            return ms.ToArray();
        }

        public Task<string> GenerateImageBase64Async(string codeText, PainterOption option)
        {
            using Image<Rgba32> img = CreateImage(codeText, option);
            string base64Text = img.ToBase64String(JpegFormat.Instance);
            return Task.FromResult(base64Text);
        }

        private Image<Rgba32> CreateImage(string codeText, PainterOption option)
        {
            Image<Rgba32> img = new(option.Width, option.Height);
            img.Mutate(Operate);
            return img;

            void Operate(IImageProcessingContext ctx)
            {
                var random = RandomHelper.CurrentRandom;
                float fontSize = RandomFontSize(option.Width, option.Height, codeText.Length);
                ctx.BackgroundColor(option.UseBlackWhiteOnly ? Color.White : _colorRandomer.RandomLight());
                //.Glow(_colorRandomer.RandomLight())
                // Draw Points
                if (option.PointCount > 0)
                {
                    for (int i = 0; i < option.PointCount; i++)
                    {
                        ctx.Draw(color: option.UseBlackWhiteOnly ? Color.Black : _colorRandomer.RandomDark(),
                            thickness: DefaultPointSize,
                            shape: new RectangleF(
                                x: random.Next(DefaultPointSize, option.Width - DefaultPointSize),
                                y: random.Next(DefaultPointSize, option.Height - DefaultPointSize),
                                width: DefaultPointSize,
                                height: DefaultPointSize)
                            );
                    }
                }
                // Draw Bubbles
                if (option.BubbleCount > 0)
                {
                    int rad = random.Next(5, 11);
                    for (int i = 0; i < option.BubbleCount; i++)
                    {
                        var ep = new EllipsePolygon(x: random.Next(rad + 1, option.Width - rad - 1),
                           y: random.Next(rad + 1, option.Height - rad - 1),
                           radius: rad);
                        ctx.Draw(option.UseBlackWhiteOnly ? Color.Black : _colorRandomer.RandomDark(), 0.2f, ep.Clip());
                    }
                }
                // Draw Chars for interference
                if (option.InterferCharCount > 0)
                {
                    float interferCharSize = fontSize * 0.36f;
                    for (int i = 0; i < option.InterferCharCount; i++)
                    {
                        ctx.DrawText(text: RandomChar().ToString(),
                            font: _fontRandomer.Random(interferCharSize, FontStyle.Regular),
                            color: option.UseBlackWhiteOnly ? Color.Black : _colorRandomer.RandomDark(),
                            location: new(random.Next(1, (int)(option.Width - interferCharSize)), random.Next(0, (int)(option.Height - interferCharSize))));
                    }
                }
                // Draw CodeText
                var fontLocations = RandomFontLocations(option.Width, option.Height, codeText.Length);
                for (int i = 0; i < codeText.Length; i++)
                {
                    ctx.DrawText(text: codeText[i].ToString(),
                        font: _fontRandomer.Random(fontSize, FontStyle.Regular),
                        color: option.UseBlackWhiteOnly ? Color.Black : _colorRandomer.RandomDark(),
                        location: fontLocations[i]);
                }
                //Draw Lines
                if (option.LineCount > 0)
                {
                    if (option.LineCount > 1)
                    {
                        for (int i = 1; i < option.LineCount; i++)
                        {
                            ctx.DrawLines(color: option.UseBlackWhiteOnly ? Color.Black : _colorRandomer.RandomDark(),
                                thickness: DefaultLineThickness,
                                new PointF(random.Next(0, option.Width / 2), random.Next(1, option.Height)),
                                new PointF(random.Next(option.Width / 2 + 1, option.Width), random.Next(1, option.Height))
                                );
                        }
                    }
                    ctx.DrawBeziers(color: option.UseBlackWhiteOnly ? Color.Black : _colorRandomer.RandomDark(),
                            thickness: DefaultLineThickness,
                            new PointF(random.Next(0, option.Width / 4), random.Next(1, option.Height)),
                            new PointF(random.Next(option.Width / 4 + 1, option.Width / 2), random.Next(1, option.Height)),
                            new PointF(random.Next(option.Width / 2 + 1, option.Width * 3 / 4), random.Next(1, option.Height)),
                            new PointF(random.Next(option.Width * 3 / 4 + 1, option.Width), random.Next(1, option.Height))
                            );
                }
                //ctx.GaussianBlur(0.4f);

                char RandomChar()
                {
                    int ascii = random.Next(48, 110);
                    if (ascii >= 84)// a - z
                    {
                        return (char)(ascii + 13);
                    }
                    if (ascii >= 58)// A - Z
                    {
                        return (char)(ascii + 7);
                    }
                    return (char)ascii;// 0 - 9
                }

                float RandomFontSize(int width, int height, int length)
                {
                    return Math.Min(width / (length + 1f), height * 0.8f) * random.Next(90, 100) * 0.01f;
                }

                PointF[] RandomFontLocations(int width, int height, int length)
                {
                    int sideOffset = random.Next(0, (int)(width * 0.1f)) + 1;
                    int interval = (width - 2 * sideOffset) / length;
                    var result = new PointF[length];
                    for (int i = 0; i < length; i++)
                    {
                        double x = sideOffset + interval * (i + 0.5d * random.NextDouble());
                        double y = height * 0.1d * random.NextDouble();
                        result[i] = new PointF((float)x, (float)y);
                    }
                    return result;
                }
            }
        }
    }
}