using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraBallPowerUp : PowerUpPresenter
    {
        [Inject]
        private BallManager _ballManager;

        protected override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("Extra ball!");
            _ballManager.CreateBonusBall.Execute(paddle.InitialBallPosTrfm.position);
        }
    }
}