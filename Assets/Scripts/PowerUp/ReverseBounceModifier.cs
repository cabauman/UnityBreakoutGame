using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ReverseBounceModifier : PowerUp
    {
        [Inject] private ReverseBounceStrategy _strategy;

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("ReverseBounceModifier");
            paddle.Presenter.SetGameWorldEffect(_strategy);
        }
    }
}