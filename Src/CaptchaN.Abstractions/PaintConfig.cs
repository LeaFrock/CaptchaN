namespace CaptchaN.Abstractions;

public class PaintConfig
{
    public int PointCount { get; set; }

    public int LineCount { get; set; }

    public int StarCount { get; set; }

    public int BubbleCount { get; set; }

    public int InterferCharCount { get; set; }

    public bool UseBlackWhiteOnly { get; set; }
}