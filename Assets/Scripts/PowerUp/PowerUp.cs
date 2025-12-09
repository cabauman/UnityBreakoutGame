using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public abstract class PowerUp
    {
        public abstract string SpriteName { get; }

        public abstract void ApplyEffect(Game game, Vector3 position);
    }
}
