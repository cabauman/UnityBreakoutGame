using UnityEngine;

namespace BreakoutGame
{
    public interface IRandom
    {
        int Next(int minValue, int maxValue);
    }

    public sealed class UnityRandom : IRandom
    {
        /// <summary>
        /// Return a random int within [minInclusive..maxExclusive).
        /// </summary>
        public int Next(int minValue, int maxValue)
        {
            return Random.Range(minValue, maxValue);
        }
    }
}