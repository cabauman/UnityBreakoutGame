using R3;
using UnityEngine;

namespace BreakoutGame
{
    public interface IBall
    {
        float InitialForce { get; }

        int Power { get; }

        ReactiveProperty<bool> Active { get; }
    }

    public interface MyComponent<T>
    {
        T Value { get; } 
    }

    // IMutableInspectorConfig
    public interface ITestable<T>
    {
        void SetConfig(T config);
    }

    public interface IView
    {
        GameObject GameObject { get; }
        U Instantiate<T, U>(T prefab, Vector3 position, Quaternion rotation)
            where T : Component, MyComponent<U>
        {
            return Object.Instantiate(prefab, position, rotation).Value;
        }
    }
}