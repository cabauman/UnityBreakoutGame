using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public interface IBall
    {
        float InitialForce { get; }

        int Power { get; }

        IReactiveProperty<bool> Active { get; }
    }

    public interface MyComponent<T>
    {
        T Value { get; } 
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