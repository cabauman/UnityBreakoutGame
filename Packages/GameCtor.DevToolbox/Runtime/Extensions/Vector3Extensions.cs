using UnityEngine;

namespace GameCtor.DevToolbox
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Returns a copy of this vector with an altered x and/or y and/or z component
        /// </summary>
        public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
        }

        public static void SetX(this Transform original, float x)
        {
            original.position = new Vector3(x, original.position.y, original.position.z);
        }

        public static void SetY(this Transform original, float y)
        {
            original.position = new Vector3(original.position.x, y, original.position.z);
        }

        public static void SetZ(this Transform original, float z)
        {
            original.position = new Vector3(original.position.x, original.position.y, z);
        }
    }
}
