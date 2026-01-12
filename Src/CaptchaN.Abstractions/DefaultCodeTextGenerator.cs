namespace CaptchaN.Abstractions;

public sealed class DefaultCodeTextGenerator : ICodeTextGenerator
{
    //Skip '0/o/O','1/l/I','9/g/q'
    private const string _pool = "2345678" + "abcdefhijkmnprstuvwxyz" + "ABCDEFGHJKLMNPQRSTUVWXYZ";

    private const int MinCodeLength = 4;
    private const int MaxCodeLength = 6;

    public string Generate(int length)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfLessThan(length, MinCodeLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, MaxCodeLength);
#else
        if (length < MinCodeLength || length > MaxCodeLength)
        {
            throw new ArgumentOutOfRangeException(nameof(length), $"Length must be between {MinCodeLength} and {MaxCodeLength}.");
        }
#endif

        return string.Create(length, Random.Shared, (span, rnd) =>
        {
#if NET8_0_OR_GREATER
            rnd.GetItems(_pool, span);
#else
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = _pool[rnd.Next(_pool.Length)];
            }
#endif
        });
    }
}