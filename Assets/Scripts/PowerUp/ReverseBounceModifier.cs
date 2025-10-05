using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ReverseBounceModifier : PowerUp
    {
        [Inject] private ReverseBounceStrategy _strategy;

        // private readonly ReverseBounceStrategy _strategy;

        // public ReverseBounceModifier(ReverseBounceStrategy strategy)
        // {
        //     _strategy = strategy;
        // }

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("ReverseBounceModifier");
            paddle.Presenter.SetBallCollisionStrategy(_strategy);
        }
    }
}