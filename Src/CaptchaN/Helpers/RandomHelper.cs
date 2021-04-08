using System;
using System.Threading;

namespace CaptchaN.Helpers
{
    internal static class RandomHelper
    {
        private readonly static ThreadLocal<Random> _randomLocal = new(() => new Random(Guid.NewGuid().GetHashCode()), false);

        public static Random CurrentRandom => _randomLocal.Value;
    }
}