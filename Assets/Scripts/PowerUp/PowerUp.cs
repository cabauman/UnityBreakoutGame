using UnityEngine;

namespace BreakoutGame
{
    public abstract class PowerUp : ScriptableObject
    {
        public Sprite Sprite;

        public abstract void ApplyEffect(PaddlePresenter paddle);
    }
}