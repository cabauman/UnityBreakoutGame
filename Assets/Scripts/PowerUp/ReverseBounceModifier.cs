using UnityEngine;

namespace BreakoutGame
{
    public sealed class ReverseBounceModifier : PowerUpAction
    {
        public override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("ReverseBounceModifier");
        }
    }
}