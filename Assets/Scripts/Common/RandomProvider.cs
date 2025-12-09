using UnityEngine;

namespace BreakoutGame
{
    public interface IRandom
    {
        /// <summary>
        /// Return a random int within [minInclusive..maxExclusive).
        /// </summary>
        int Next(int minValue, int maxValue);

        /// <summary>
        /// Return a random float within [minInclusive..maxInclusive).
        /// </summary>
        float Next(float minValue, float maxValue);
    }

    public sealed class UnityRandom : IRandom
    {
        /// <inheritdoc />
        public int Next(int minValue, int maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        /// <inheritdoc />
        public float Next(float minValue, float maxValue)
        {
            return Random.Range(minValue, maxValue);
        }
    }
}
