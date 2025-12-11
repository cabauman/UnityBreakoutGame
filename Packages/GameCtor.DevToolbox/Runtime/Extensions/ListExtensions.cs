using System.Collections.Generic;

namespace GameCtor.DevToolbox
{
    public static class ListExtensions
    {
        /// <summary>
        /// Shuffles the list using the Fisher–Yates algorithm.
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle</remarks>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 1; i--)
            {
                int k = RandomUtil.Random.Next(i);
                (list[i], list[k]) = (list[k], list[i]);
            }
        }
    }
}
