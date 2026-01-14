using System.Numerics;
using CaptchaN.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using Constants = CaptchaN.Abstractions.CaptchaConstants;

namespace CaptchaN.Drawing.ImageSharp;

public partial class Painter
{
    private readonly struct MaoPen(IImageProcessingContext imgContext,
        Random random,
        MaoPenConfig config)
    {
        public MaoPen Background()
        {
            var bg = config.PaintConfig.UseBlackWhiteOnly
                ? Color.White
                : Colors.RandomLight(random);
            imgContext.BackgroundColor(bg);
            return this;
        }

        public MaoPen DrawCode()
        {
            var code = config.CodeText;
            var fontSize = config.PainterOption.MaxFontSize * random.Next(90, 101) / 100;
            var blockWidth = Constants.DefaultWidth / code.Length;
            for (int i = 0; i < code.Length; i++)
            {
                var charText = char.ToString(code[i]);
                var font = Fonts.RandomPick(random, fontSize);
                var x = RandomOffset(config.PainterOption.FontOffsetXRange) + i * blockWidth;
                var y = RandomOffset(config.PainterOption.FontOffsetYRange);
                imgContext.DrawText(text: charText,
                        font: font,
                        color: RandomDark(random),
                        location: new(x, y));
            }
            return this;
        }

        public MaoPen DrawPoints()
        {
            var pointCount = config.PaintConfig.PointCount;
            if (pointCount > 0)
            {
                for (int i = 0; i < pointCount; i++)
                {
                    var pointSize = RandomShapeSize(0.1d);
                    var location = RandomTopLeftLocation(pointSize);
                    imgContext.Fill(color: RandomDark(random, 0.4f),
                        shape: new RectangleF(point: location, size: new(pointSize, pointSize)));
                }
            }
            return this;
        }

        public MaoPen DrawBubbles()
        {
            var bubbleCount = config.PaintConfig.BubbleCount;
            if (bubbleCount > 0)
            {
                for (int i = 0; i < bubbleCount; i++)
                {
                    var r = RandomShapeSize(0.2d);
                    imgContext.Draw(color: RandomDark(random, 0.5f),
                        thickness: 1f,
                        path: new EllipsePolygon(location: RandomCenterLocation(r), radius: r));
                }
            }
            return this;
        }

        public MaoPen DrawStars()
        {
            var starCount = config.PaintConfig.StarCount;
            if (starCount > 0)
            {
                var prongsMask = (ulong)random.NextInt64();
                for (int i = 0; i < starCount; i++)
                {
                    var prongs = (int)(prongsMask & 0b11) + 3; // 3~6
                    var r = RandomShapeSize(0.2d);
                    imgContext.Draw(color: RandomDark(random, 0.6f),
                        thickness: 1f,
                        path: new Star(location: RandomCenterLocation(r), prongs: prongs, innerRadii: r * 0.01f * random.Next(32, 81), outerRadii: r));
                    prongsMask = BitOperations.RotateRight(prongsMask, 2);
                }
            }
            return this;
        }

        public MaoPen DrawLines()
        {
            var lineCount = config.PaintConfig.LineCount;
            if (lineCount > 0)
            {
                var blockWidth = Constants.DefaultWidth / 4;
                var bezierPoints = new PointF[4];
                bezierPoints[0] = new(2, RandomY(random));
                bezierPoints[1] = new((int)(blockWidth * (1 + random.NextDouble())), RandomY(random));
                bezierPoints[2] = new((int)(blockWidth * (2 + random.NextDouble())), RandomY(random));
                bezierPoints[3] = new(Constants.DefaultWidth - 2, RandomY(random));
                imgContext.DrawBeziers(color: RandomDark(random, 0.75f),
                    thickness: 2,
                    points: bezierPoints);

                blockWidth = Constants.DefaultWidth / 8;
                for (int i = 1; i < lineCount; i++)
                {
                    var left = new PointF(x: random.Next(1, blockWidth), y: RandomY(random));
                    var center = new PointF(x: random.Next(blockWidth * 3, blockWidth * 5), y: RandomY(random));
                    var right = new PointF(x: random.Next(Constants.DefaultWidth - blockWidth, Constants.DefaultWidth), y: RandomY(random));
                    imgContext.DrawLine(color: RandomDark(random, 0.8f),
                        thickness: 2,
                        points: [left, center, right]);
                }
            }
            return this;

            static int RandomY(Random rand) => rand.Next(1, Constants.DefaultHeight);
        }

        public MaoPen DrawInterferChars()
        {
            var interferCharCount = config.PaintConfig.InterferCharCount;
            if (interferCharCount > 0)
            {
                var fontSize = RandomInterferCharFontSize(random, config.PainterOption.MaxFontSize);
                for (int i = 0; i < interferCharCount; i++)
                {
                    var charText = char.ToString(Constants.Alphabet[random.Next(Constants.Alphabet.Length)]);
                    var font = Fonts.RandomPick(random, fontSize);
                    var location = RandomTopLeftLocation(fontSize * 3 / 2);
                    imgContext.DrawText(text: charText,
                        font: font,
                        color: RandomDark(random),
                        location: location);
                }
            }
            return this;

            static int RandomInterferCharFontSize(Random r, in int maxFontSize) => (int)Math.Ceiling(maxFontSize * 0.15d * (1 + r.NextDouble()));
        }

        public void Resize()
        {
            var size = config.Size;
            if (size.Width < Constants.DefaultWidth && size.Height < Constants.DefaultHeight)
            {
                imgContext.Resize(size.Width, size.Height, KnownResamplers.Bicubic);
            }
        }

        private Color RandomDark(Random r) => config.PaintConfig.UseBlackWhiteOnly
            ? Color.Black
            : Colors.RandomDark(r);

        private Color RandomDark(Random r, float alpha) => RandomDark(r).WithAlpha(alpha);

        private int RandomShapeSize(double ratio)
            => (int)Math.Ceiling(Constants.DefaultHeight * ratio * (1 + random.NextDouble()));

        private PointF RandomTopLeftLocation(int shapeSize, int paddingSize = 1)
        {
            var x = random.Next(paddingSize, Constants.DefaultWidth - shapeSize - paddingSize);
            var y = random.Next(paddingSize, Constants.DefaultHeight - shapeSize - paddingSize);
            return new(x, y);
        }

        private PointF RandomCenterLocation(int radius, int paddingSize = 1)
        {
            var x = random.Next(radius + paddingSize, Constants.DefaultWidth - radius - paddingSize);
            var y = random.Next(radius + paddingSize, Constants.DefaultHeight - radius - paddingSize);
            return new(x, y);
        }

        private int RandomOffset(int[] offset) => random.Next(offset[0], offset[1] + 1);
    }

    private sealed class MaoPenConfig
    {
        public string CodeText { get; init; } = string.Empty;

        public ImageSize Size { get; init; }

        public PaintConfig PaintConfig { get; init; } = default!;

        public PainterOption PainterOption { get; init; } = default!;
    }
}
