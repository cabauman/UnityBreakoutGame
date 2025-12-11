using System.Collections.Concurrent;
using System.Text;

namespace GameCtor.DevToolbox
{
    public static class StringBuilderPool
    {
        private static readonly ConcurrentBag<StringBuilder> _pool = new();

        /// <summary>
        /// Gets a StringBuilder from the pool, or creates a new one if none are available.
        /// The returned StringBuilder is guaranteed to be empty.
        /// </summary>
        public static StringBuilder Get()
        {
            if (_pool.TryTake(out var sb))
            {
                sb.Clear();
                return sb;
            }

            return new StringBuilder();
        }

        /// <summary>
        /// Returns a StringBuilder to the pool for reuse.
        /// </summary>
        public static void Return(StringBuilder sb)
        {
            if (sb is not null)
            {
                _pool.Add(sb);
            }
        }
    }
}
