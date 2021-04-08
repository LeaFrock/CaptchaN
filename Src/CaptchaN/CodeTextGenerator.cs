using System;
using CaptchaN.Abstractions;
using CaptchaN.Helpers;

namespace CaptchaN
{
    public class CodeTextGenerator : ICodeTextGenerator
    {
        private readonly static char[] _pool = new char[]
        {
            //Skip '0/o/O','1/l/I','9/g/q'
            '2','3','4','5','6','7','8',
            'a','b','c','d','e','f','h','i','j','k','m','n','p','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z',
        };

        public string Generate(int length)
        {
            var random = RandomHelper.CurrentRandom;
            Span<char> span = length <= 10 ? stackalloc char[length] : new char[length];
            for (int i = 0; i < length; i++)
            {
                span[i] = _pool[random.Next(0, _pool.Length)];
            }
            return new string(span);
        }
    }
}