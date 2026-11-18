using System.Numerics;
using CaptchaN.Abstractions;
using NetVips;
using static NetVips.Enums;

namespace CaptchaN.Drawing.LibVips;

public partial class Painter
{
    private sealed class MaoPen(Image image) : IDisposable
    {
        private Image? image = image;

        private Image Current => image ?? throw new ObjectDisposedException(nameof(MaoPen));

        public required string CodeText { get; init; }

        public required ImageSize Size { get; init; }

        public required PaintConfig PaintConfig { get; init; }

        public required PainterOption PainterOption { get; init; }

        public required VipsColor BackgroundColor { get; init; }

        public required Random Random { get; init; }

        public MaoPen DrawCode()
        {
            var code = CodeText;
            var fontSize = PainterOption.MaxFontSize * Random.Next(90, 101) / 100;
            var blockWidth = CaptchaConstants.DefaultWidth / code.Length;
            for (int i = 0; i < code.Length; i++)
            {
                var charText = char.ToString(code[i]);
                var x = RandomOffset(PainterOption.FontOffsetXRange) + i * blockWidth;
                var y = RandomOffset(PainterOption.FontOffsetYRange);
                var font = Fonts.RandomPick(Random);
                DrawText(charText, x, y, fontSize, RandomDark(), font);
            }
            return this;
        }

        public MaoPen DrawPoints()
        {
            var pointCount = PaintConfig.PointCount;
            for (int i = 0; i < pointCount; i++)
            {
                var pointSize = RandomShapeSize(0.1d);
                var (x, y) = RandomTopLeftLocation(pointSize);
                using var point = CreateCanvas(pointSize, pointSize, RandomDark(128));
                Composite(point, x, y);
            }
            return this;
        }

        public MaoPen DrawBubbles()
        {
            var bubbleCount = PaintConfig.BubbleCount;
            for (int i = 0; i < bubbleCount; i++)
            {
                var r = RandomShapeSize(0.2d);
                var (x, y) = RandomCenterLocation(r);
                using var canvas = CreateCanvas(2 * r + 1, 2 * r + 1, default);
                using var bubble = canvas.Mutate(mutable =>
                    mutable.DrawCircle(RandomDark(160).ToDoubleArray(), r, r, r, fill: false));
                Composite(bubble, x - r, y - r);
            }
            return this;
        }

        public MaoPen DrawStars()
        {
            var starCount = PaintConfig.StarCount;
            if (starCount > 0)
            {
                var prongsMask = (ulong)Random.NextInt64();
                var vertBuffer = new PointD[6 * 2];
                for (int i = 0; i < starCount; i++)
                {
                    var prongs = (int)(prongsMask & 0b11) + 3;
                    var r = RandomShapeSize(0.2d);
                    var (x, y) = RandomCenterLocation(r);
                    var vertCount = Star(vertBuffer, x, y, prongs, r * 0.01d * Random.Next(32, 81), r);
                    var ink = RandomDark(180).ToDoubleArray();
                    using var canvas = CreateCanvas(CaptchaConstants.DefaultWidth, CaptchaConstants.DefaultHeight, default);
                    using var star = canvas.Mutate(mutable =>
                    {
                        for (int j = 0; j < vertCount; j++)
                        {
                            var start = vertBuffer[j];
                            var end = vertBuffer[(j + 1) % vertCount];
                            mutable.DrawLine(ink, (int)start.X, (int)start.Y, (int)end.X, (int)end.Y);
                        }
                    });
                    Composite(star, 0, 0);
                    prongsMask = BitOperations.RotateRight(prongsMask, 2);
                }
            }
            return this;

            static int Star(Span<PointD> verts, in double cx, in double cy, in int prongs, in double innerR, in double outerR)
            {
                var total = prongs * 2;
                for (int i = 0; i < total; i++)
                {
                    var angle = (double)i / prongs * Math.PI;
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
                using (var canvas = CreateCanvas(CaptchaConstants.DefaultWidth, CaptchaConstants.DefaultHeight, default))
                using (var line = canvas.Mutate(mutable => DrawBezier(mutable, RandomDark(200).ToDoubleArray(), bezierPoints)))
                {
                    Composite(line, 0, 0);
                }

                blockWidth = CaptchaConstants.DefaultWidth / 8;
                for (int i = 1; i < lineCount; i++)
                {
                    var left = new PointD(X: random.Next(1, blockWidth), Y: RandomY(random));
                    var center = new PointD(X: random.Next(blockWidth * 3, blockWidth * 5), Y: RandomY(random));
                    var right = new PointD(X: random.Next(CaptchaConstants.DefaultWidth - blockWidth, CaptchaConstants.DefaultWidth), Y: RandomY(random));
                    var ink = RandomDark(200).ToDoubleArray();
                    using var canvas = CreateCanvas(CaptchaConstants.DefaultWidth, CaptchaConstants.DefaultHeight, default);
                    using var line = canvas.Mutate(mutable =>
                    {
                        DrawThickLine(mutable, ink, left, center);
                        DrawThickLine(mutable, ink, center, right);
                    });
                    Composite(line, 0, 0);
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
                for (int i = 0; i < interferCharCount; i++)
                {
                    var charText = char.ToString(RandomInterferChar(Random, CaptchaConstants.Alphabet));
                    var font = Fonts.RandomPick(Random);
                    var (x, y) = RandomCenterLocation(fontSize * 3 / 2);
                    DrawText(charText, x, y, fontSize, RandomDark(200), font);
                }
            }
            return this;

            static int RandomInterferCharFontSize(Random r, in int maxFontSize) => (int)Math.Ceiling(maxFontSize * 0.15d * (1 + r.NextDouble()));

            static char RandomInterferChar(Random r, string alphabet) => alphabet[r.Next(alphabet.Length)];
        }

        public Image Resize()
        {
            using var source = Detach();
            var flattened = source.Flatten(background: BackgroundColor.ToRgbArray());
            var size = Size;
            if (size.Width < CaptchaConstants.DefaultWidth && size.Height < CaptchaConstants.DefaultHeight)
            {
                using (flattened)
                {
                    return flattened.Resize(
                        (double)size.Width / CaptchaConstants.DefaultWidth,
                        vscale: (double)size.Height / CaptchaConstants.DefaultHeight);
                }
            }
            return flattened;
        }

        public void Dispose()
        {
            image?.Dispose();
            image = null;
        }

        private void DrawText(string text, int x, int y, int fontSize, VipsColor color, VipsFont font)
        {
            using var mask = font.FilePath is null
                ? Image.Text(text, font: $"{font.Family} {fontSize}")
                : Image.Text(text, font: $"{font.Family} {fontSize}", fontfile: font.FilePath);
            using var canvas = CreateCanvas(mask.Width, mask.Height, default);
            using var textImage = canvas.Mutate(mutable =>
                mutable.DrawMask(color.ToDoubleArray(), mask, 0, 0));
            Composite(textImage, x, y - mask.Height);
        }

        private static void DrawBezier(MutableImage image, double[] ink, ReadOnlySpan<PointD> points)
        {
            const int segments = 32;
            var previous = points[0];
            for (int i = 1; i <= segments; i++)
            {
                var t = (double)i / segments;
                var u = 1 - t;
                var current = new PointD(
                    u * u * u * points[0].X + 3 * u * u * t * points[1].X + 3 * u * t * t * points[2].X + t * t * t * points[3].X,
                    u * u * u * points[0].Y + 3 * u * u * t * points[1].Y + 3 * u * t * t * points[2].Y + t * t * t * points[3].Y);
                DrawThickLine(image, ink, previous, current);
                previous = current;
            }
        }

        private static void DrawThickLine(MutableImage image, double[] ink, PointD start, PointD end)
        {
            var x1 = (int)start.X;
            var y1 = (int)start.Y;
            var x2 = (int)end.X;
            var y2 = (int)end.Y;
            image.DrawLine(ink, x1, y1, x2, y2);
            image.DrawLine(ink, x1, y1 + 1, x2, y2 + 1);
        }

        private void Composite(Image overlay, int x, int y)
        {
            var composited = Current.Composite2(overlay, BlendMode.Over, x: x, y: y);
            Current.Dispose();
            image = composited;
        }

        private Image Detach()
        {
            var current = Current;
            image = null;
            return current;
        }

        private VipsColor RandomDark() => PaintConfig.UseBlackWhiteOnly ? Colors.Black : Colors.RandomDark(Random);

        private VipsColor RandomDark(byte alpha) => RandomDark().WithAlpha(alpha);

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

        private readonly record struct PointD(double X, double Y);
    }
}
