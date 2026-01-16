using System.Numerics;
using CaptchaN.Abstractions;
using SkiaSharp;
using Constants = CaptchaN.Abstractions.CaptchaConstants;

namespace CaptchaN.Drawing.SkiaSharp;

public partial class Painter
{
    private readonly struct MaoPen(SKBitmap bitmap)
    {
        private readonly SKCanvas canvas = new(bitmap);
        private readonly SKPaint paint = new() { IsAntialias = true };

        public string CodeText { get; init; } = string.Empty;

        public ImageSize Size { get; init; }

        public PaintConfig PaintConfig { get; init; } = default!;

        public PainterOption PainterOption { get; init; } = default!;

        public Random Random { get; init; } = default!;

        public MaoPen Background()
        {
            var bg = PaintConfig.UseBlackWhiteOnly ? SKColors.White : Colors.RandomLight(Random);
            canvas.Clear(bg);
            return this;
        }

        public MaoPen DrawCode()
        {
            var code = CodeText;
            var fontSize = PainterOption.MaxFontSize * Random.Next(90, 101) / 100;
            var blockWidth = Constants.DefaultWidth / code.Length;
            for (int i = 0; i < code.Length; i++)
            {
                var charText = char.ToString(code[i]);
                using var font = Fonts.RandomPick(Random, fontSize);
                var x = RandomOffset(PainterOption.FontOffsetXRange) + i * blockWidth;
                var y = RandomOffset(PainterOption.FontOffsetYRange);
                paint.Color = RandomDark();
                canvas.DrawText(text: charText, x, y, textAlign: SKTextAlign.Left, font, paint);
            }
            return this;
        }

        public MaoPen DrawPoints()
        {
            var pointCount = PaintConfig.PointCount;
            if (pointCount > 0)
            {
                paint.Style = SKPaintStyle.Fill;
                paint.IsAntialias = false;
                for (int i = 0; i < pointCount; i++)
                {
                    var pointSize = RandomShapeSize(0.1d);
                    var (x, y) = RandomTopLeftLocation(pointSize);
                    paint.Color = RandomDark(128);
                    canvas.DrawRect(x, y, pointSize, pointSize, paint);
                }
                paint.IsAntialias = true;
            }
            return this;
        }

        public MaoPen DrawBubbles()
        {
            var bubbleCount = PaintConfig.BubbleCount;
            if (bubbleCount > 0)
            {
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = 1;
                for (int i = 0; i < bubbleCount; i++)
                {
                    var r = RandomShapeSize(0.2d);
                    (var x, var y) = RandomCenterLocation(r);
                    paint.Color = RandomDark(160);
                    canvas.DrawCircle(x, y, r, paint);
                }
            }
            return this;
        }

        public MaoPen DrawStars()
        {
            var starCount = PaintConfig.StarCount;
            if (starCount > 0)
            {
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = 1;
                var prongsMask = (ulong)Random.NextInt64();
                for (int i = 0; i < starCount; i++)
                {
                    var prongs = (int)(prongsMask & 0b11) + 3; // 3~6
                    var r = RandomShapeSize(0.2d);
                    var (x, y) = RandomCenterLocation(r);
                    paint.Color = RandomDark(180);
                    Star(canvas, paint, x, y, prongs: prongs, innerR: r * 0.01f * Random.Next(32, 81), outerR: r);
                    prongsMask = BitOperations.RotateRight(prongsMask, 2);
                }
            }
            return this;

            static void Star(SKCanvas c, SKPaint p, float cx, float cy, int prongs, float innerR, float outerR)
            {
                using var path = new SKPath();
                path.MoveTo(cx + outerR, cy);
                float angleStep = 360f / prongs;
                for (int i = 1; i <= prongs * 2; i++)
                {
                    var angle = i * angleStep * 0.5f * MathF.PI / 180f;
                    var r = (i & 1) == 0 ? outerR : innerR;
                    var x = cx + r * MathF.Cos(angle);
                    var y = cy + r * MathF.Sin(angle);
                    path.LineTo(x, y);
                }
                path.Close();
                c.DrawPath(path, p);
            }
        }

        public MaoPen DrawLines()
        {
            var lineCount = PaintConfig.LineCount;
            if (lineCount > 0)
            {
                var random = Random;
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = 2;
                using var path = new SKPath();
                var blockWidth = Constants.DefaultWidth / 4;
                path.MoveTo(2, RandomY(random));
                path.CubicTo(
                    new((int)(blockWidth * (1 + random.NextDouble())), RandomY(random)),
                    new((int)(blockWidth * (2 + random.NextDouble())), RandomY(random)),
                    new(Constants.DefaultWidth - 2, RandomY(random)));
                paint.Color = RandomDark(200);
                canvas.DrawPath(path, paint);

                blockWidth = Constants.DefaultWidth / 8;
                for (int i = 1; i < lineCount; i++)
                {
                    path.Reset();
                    path.MoveTo(x: random.Next(1, blockWidth), y: RandomY(random));
                    path.LineTo(x: random.Next(blockWidth * 3, blockWidth * 5), y: RandomY(random));
                    path.LineTo(x: random.Next(Constants.DefaultWidth - blockWidth, Constants.DefaultWidth), y: RandomY(random));
                    paint.Color = RandomDark(200);
                    canvas.DrawPath(path, paint);
                }
            }
            return this;

            static int RandomY(Random rand) => rand.Next(1, Constants.DefaultHeight);
        }

        public MaoPen DrawInterferChars()
        {
            var interferCharCount = PaintConfig.InterferCharCount;
            if (interferCharCount > 0)
            {
                var fontSize = RandomInterferCharFontSize(Random, PainterOption.MaxFontSize);
                for (int i = 0; i < interferCharCount; i++)
                {
                    var charText = char.ToString(RandomInterferChar(Random, Constants.Alphabet));
                    using var font = Fonts.RandomPick(Random, fontSize);
                    (var x, var y) = RandomCenterLocation(fontSize * 3 / 2);
                    paint.Color = RandomDark(200);
                    canvas.DrawText(text: charText, x, y, textAlign: SKTextAlign.Left, font, paint);
                }
            }
            return this;

            static int RandomInterferCharFontSize(Random r, in int maxFontSize) => (int)Math.Ceiling(maxFontSize * 0.15d * (1 + r.NextDouble()));

            static char RandomInterferChar(Random r, string alphabet) => alphabet[r.Next(alphabet.Length)];
        }

        public SKBitmap Resize()
        {
            canvas.Dispose();
            paint.Dispose();
            var size = Size;
            if (size.Width < Constants.DefaultWidth && size.Height < Constants.DefaultHeight)
            {
                return bitmap.Resize(new SKImageInfo(size.Width, size.Height), SKSamplingOptions.Default);
            }
            return bitmap;
        }

        private SKColor RandomDark() => PaintConfig.UseBlackWhiteOnly
            ? SKColors.Black
            : Colors.RandomDark(Random);

        private SKColor RandomDark(byte alpha) => RandomDark().WithAlpha(alpha);

        private int RandomShapeSize(double ratio)
            => (int)Math.Ceiling(Constants.DefaultHeight * ratio * (1 + Random.NextDouble()));

        private (float x, float y) RandomTopLeftLocation(int shapeSize, int paddingSize = 1)
        {
            var x = Random.Next(paddingSize, Constants.DefaultWidth - shapeSize - paddingSize);
            var y = Random.Next(paddingSize, Constants.DefaultHeight - shapeSize - paddingSize);
            return (x, y);
        }

        private (float x, float y) RandomCenterLocation(int radius, int paddingSize = 1)
        {
            var x = Random.Next(radius + paddingSize, Constants.DefaultWidth - radius - paddingSize);
            var y = Random.Next(radius + paddingSize, Constants.DefaultHeight - radius - paddingSize);
            return (x, y);
        }

        private int RandomOffset(int[] offset) => Random.Next(offset[0], offset[1] + 1);
    }
}
