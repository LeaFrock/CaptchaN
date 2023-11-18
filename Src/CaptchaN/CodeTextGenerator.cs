using CaptchaN.Abstractions;

namespace CaptchaN
{
    public class CodeTextGenerator : ICodeTextGenerator
    {
        //Skip '0/o/O','1/l/I','9/g/q'
        private const string _pool = "2345678"
            + "abcdefhijkmnprstuvwxyz"
            + "ABCDEFGHJKLMNPQRSTUVWXYZ";

        public string Generate(int length)
        {
            var random = Random.Shared;
            Span<char> span = length <= 10
                ? stackalloc char[length]
                : new char[length]; // Will you do so?
#if NET8_0_OR_GREATER
            random.GetItems(_pool, span);
#else
            for (int i = 0; i < length; i++)
            {
                span[i] = _pool[random.Next(_pool.Length)];
            }
#endif
            return new string(span);
        }
    }
}