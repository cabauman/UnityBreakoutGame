using UnityEngine;

namespace GameCtor.DevToolbox
{
    public static class UnityObjectExtensions
    {
        /// <summary>
        /// Returns null if the Unity Object has been destroyed, otherwise returns the object itself.
        /// Enables valid use of null-conditional access (?.) and null-coalescing (??) operators.
        /// </summary>
        /// <remarks>
        /// This is useful because Unity overrides the equality operator for UnityEngine.Object.
        /// Even if obj1 and obj2 are not technically null in C#
        /// (they still hold a reference), Unity's overridden == operator will treat them as "null"
        /// because their underlying native object has been destroyed.
        /// <code>
        /// myGameObject.OrNull()?GetComponent<MyComponent>().DoSomething();
        /// myGameObject = otherGameObject.OrNull() ?? Instantiate(myPrefab);
        /// </code>
        public static Object OrNull(this Object @this)
        {
            return @this == null ? null : @this;
        }
    }
}
