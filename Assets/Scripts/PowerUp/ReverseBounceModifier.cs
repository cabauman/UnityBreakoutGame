using UnityEngine;

namespace BreakoutGame
{
    public sealed class ReverseBounceModifier : PowerUpAction
    {
        private readonly ReverseBounceStrategy _strategy;

        public ReverseBounceModifier(ReverseBounceStrategy strategy)
        {
            _strategy = strategy;
        }

        public override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("ReverseBounceModifier");
            paddle.Presenter.SetCollisionStrategy(_strategy);
        }
    }
}