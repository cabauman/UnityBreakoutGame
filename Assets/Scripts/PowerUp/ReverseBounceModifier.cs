using UnityEngine;

namespace BreakoutGame
{
    public class ReverseBounceModifier : PowerUpPresenter
    {
        protected override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("ReverseBounceModifier");
        }
    }
}