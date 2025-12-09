using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Ball
    {
        public Ball(float initialForce, int power, Vector3 startPosition)
        {
            InitialForce = initialForce;
            Power = power;
            StartPosition = startPosition;
            Active = new ReactiveProperty<bool>(true);
        }

        public float InitialForce { get; }

        public int Power { get; }

        public Vector3 StartPosition { get; }

        public IReactiveProperty<bool> Active { get; }
    }
}
