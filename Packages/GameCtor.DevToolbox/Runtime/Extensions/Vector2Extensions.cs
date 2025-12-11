using UnityEngine;

namespace GameCtor.DevToolbox
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns a copy of this vector with an altered x component
        /// </summary>
        public static Vector2 WithX(this Vector2 original, float x)
        {
            return new Vector2(x, original.y);
        }

        /// <summary>
        /// Returns a copy of this vector with an altered y component
        /// </summary>
        public static Vector2 WithY(this Vector2 original, float y)
        {
            return new Vector2(original.x, y);
        }
    }
}
