namespace CaptchaN.Abstractions
{
    public class PainterOption
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int PointCount { get; set; }

        public int LineCount { get; set; }

        public int BubbleCount { get; set; }

        public int InterferCharCount { get; set; }

        public bool UseBlackWhiteOnly { get; set; }
    }
}