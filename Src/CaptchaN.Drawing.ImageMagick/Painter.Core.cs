using System.Numerics;
using CaptchaN.Abstractions;
using ImageMagick;
using ImageMagick.Drawing;

namespace CaptchaN.Drawing.ImageMagick;

public partial class Painter
{
    private readonly struct MaoPen(
        IMagickImage<byte> image,
        IDrawables<byte> drawables)
    {
        public required string CodeText { get; init; }

        public required ImageSize Size { get; init; }

        public required PaintConfig PaintConfig { get; init; }

        public required PainterOption PainterOption { get; init; }

        public required Random Random { get; init; }

        public MaoPen DrawCode()
        {
            var code = CodeText;
            var fontSize = PainterOption.MaxFontSize * Random.Next(90, 101) / 100;
            var blockWidth = CaptchaConstants.DefaultWidth / code.Length;
            drawables.StrokeWidth(1);
            for (int i = 0; i < code.Length; i++)
            {
                var charText = char.ToString(code[i]);
                var x = RandomOffset(PainterOption.FontOffsetXRange) + i * blockWidth;
                var y = RandomOffset(PainterOption.FontOffsetYRange);
                var fontColor = RandomDark();
                drawables
                    .Font(Fonts.RandomPick(Random))
                    .FontPointSize(fontSize)
                    .FillColor(fontColor)
                    .StrokeColor(fontColor)
                    .Text(x, y, charText);
            }
            return this;
        }

        public MaoPen DrawPoints()
        {
            var pointCount = PaintConfig.PointCount;
            if (pointCount > 0)
            {
                drawables.StrokeColor(MagickColors.Transparent);
                for (int i = 0; i < pointCount; i++)
                {
                    var pointSize = RandomShapeSize(0.1d);
                    var (x, y) = RandomTopLeftLocation(pointSize);
                    drawables
                        .FillColor(RandomDark(128))
                        .Rectangle(x, y, x + pointSize, y + pointSize);
                }
            }
            return this;
        }

        public MaoPen DrawBubbles()
        {
            var bubbleCount = PaintConfig.BubbleCount;
            if (bubbleCount > 0)
            {
                drawables
                        .FillColor(MagickColors.Transparent)
                        .StrokeWidth(1);
                for (int i = 0; i < bubbleCount; i++)
                {
                    var r = RandomShapeSize(0.15d);
                    var (x, y) = RandomCenterLocation(r);
                    drawables
                        .StrokeColor(RandomDark(160))
                        .Circle(x, y, x + r, y);
                }
            }
            return this;
        }

        public MaoPen DrawStars()
        {
            var starCount = PaintConfig.StarCount;
            if (starCount > 0)
            {
                drawables
                    .FillColor(MagickColors.Transparent)
                    .StrokeWidth(1);
                var prongsMask = (ulong)Random.NextInt64();
                Span<PointD> vertBuffer = stackalloc PointD[6 * 2]; // max 6 prongs
                for (int i = 0; i < starCount; i++)
                {
                    var prongs = (int)(prongsMask & 0b11) + 3; // 3~6
                    var r = RandomShapeSize(0.2d);
                    var (x, y) = RandomCenterLocation(r);
                    var vertCount = Star(vertBuffer, x, y, prongs, r * 0.01d * Random.Next(32, 81), r);
                    drawables
                        .StrokeColor(RandomDark(180))
                        .Polygon(vertBuffer[..vertCount].ToArray()); // TODO: params ROS not supported yet
                    prongsMask = BitOperations.RotateRight(prongsMask, 2);
                }
            }
            return this;

            static int Star(Span<PointD> verts, in double cx, in double cy, in int prongs, in double innerR, in double outerR)
            {
                var total = prongs * 2;
                for (int i = 0; i < total; i++)
                {
                    var angle = ((double)i / prongs - 0.5d) * Math.PI;   // 从顶点开始
                    var r = (i & 1) == 0 ? outerR : innerR;
                    verts[i] = new PointD(cx + r * Math.Cos(angle), cy + r * Math.Sin(angle));
                }
                return total;
            }
        }

        public MaoPen DrawLines()
        {
            var lineCount = PaintConfig.LineCount;
            if (lineCount > 0)
            {
                var random = Random;
                var blockWidth = CaptchaConstants.DefaultWidth / 4;
                var bezierPoints = new PointD[4];
                bezierPoints[0] = new(2, RandomY(random));
                bezierPoints[1] = new((int)(blockWidth * (1 + random.NextDouble())), RandomY(random));
                bezierPoints[2] = new((int)(blockWidth * (2 + random.NextDouble())), RandomY(random));
                bezierPoints[3] = new(CaptchaConstants.DefaultWidth - 2, RandomY(random));
                drawables.StrokeWidth(2)
                    .StrokeColor(RandomDark())
                    .Bezier(bezierPoints);

                blockWidth = CaptchaConstants.DefaultWidth / 8;
                for (int i = 1; i < lineCount; i++)
                {
                    var left = new PointD(x: random.Next(1, blockWidth), y: RandomY(random));
                    var center = new PointD(x: random.Next(blockWidth * 3, blockWidth * 5), y: RandomY(random));
                    var right = new PointD(x: random.Next(CaptchaConstants.DefaultWidth - blockWidth, CaptchaConstants.DefaultWidth), y: RandomY(random));
                    drawables
                        .StrokeColor(RandomDark(200))
                        .Polyline([left, center, right]);
                }
            }
            return this;

            static int RandomY(Random rand) => rand.Next(1, CaptchaConstants.DefaultHeight);
        }

        public MaoPen DrawInterferChars()
        {
            var interferCharCount = PaintConfig.InterferCharCount;
            if (interferCharCount > 0)
            {
                var fontSize = RandomInterferCharFontSize(Random, PainterOption.MaxFontSize);
                var font = Fonts.RandomPick(Random);
                for (int i = 0; i < interferCharCount; i++)
                {
                    var charText = char.ToString(RandomInterferChar(Random, CaptchaConstants.Alphabet));
                    var (x, y) = RandomCenterLocation(fontSize * 3 / 2);
                    var fontColor = RandomDark(160);
                    drawables
                        .Font(font)
                        .FontPointSize(fontSize)
                        .FillColor(fontColor)
                        .StrokeColor(fontColor)
                        .Text(x, y, charText);
                }
            }
            return this;

            static int RandomInterferCharFontSize(Random r, in int maxFontSize) => (int)Math.Ceiling(maxFontSize * 0.15d * (1 + r.NextDouble()));

            static char RandomInterferChar(Random r, string alphabet) => alphabet[r.Next(alphabet.Length)];
        }

        public void Resize()
        {
            drawables.Draw(image);
            if (Size.Width < CaptchaConstants.DefaultWidth && Size.Height < CaptchaConstants.DefaultHeight)
            {
                image.Resize((uint)Size.Width, (uint)Size.Height);
            }
        }

        private MagickColor RandomDark() => PaintConfig.UseBlackWhiteOnly ? MagickColors.Black : Colors.RandomDark(Random);

        private MagickColor RandomDark(byte alpha) => PaintConfig.UseBlackWhiteOnly ? Colors.WithAlpha(MagickColors.Black, alpha) : Colors.RandomDark(Random, alpha);

        private int RandomShapeSize(double ratio) => (int)Math.Ceiling(CaptchaConstants.DefaultHeight * ratio * (1 + Random.NextDouble()));

        private (int x, int y) RandomTopLeftLocation(int shapeSize, int paddingSize = 1)
        {
            var x = Random.Next(paddingSize, CaptchaConstants.DefaultWidth - shapeSize - paddingSize);
            var y = Random.Next(paddingSize, CaptchaConstants.DefaultHeight - shapeSize - paddingSize);
            return new(x, y);
        }

        private (int x, int y) RandomCenterLocation(int radius, int paddingSize = 1)
        {
            var x = Random.Next(radius + paddingSize, CaptchaConstants.DefaultWidth - radius - paddingSize);
            var y = Random.Next(radius + paddingSize, CaptchaConstants.DefaultHeight - radius - paddingSize);
            return new(x, y);
        }

        private int RandomOffset(int[] offset) => Random.Next(offset[0], offset[1] + 1);
    }
}
