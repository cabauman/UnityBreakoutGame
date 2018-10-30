using System;
using UnityEngine;

public static class Vector3Extensions
{
    public static void SetX(this Vector3 @this, float x)
    {
        @this = new Vector3(x, @this.y, @this.z);
    }

    public static void SetY(this Vector3 @this, float y)
    {
        @this = new Vector3(@this.x, y, @this.z);
    }
}
