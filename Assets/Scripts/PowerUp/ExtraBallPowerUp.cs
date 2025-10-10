using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraBallPowerUpAction : PowerUp
    {
        [Inject] private BallManager _ballManager;

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("Extra ball!");
            _ballManager.CreateBonusBall.Execute(paddle.InitialBallPosTrfm.position);
        }
    }
}