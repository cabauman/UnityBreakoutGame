using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraBallPowerUp : PowerUp
    {
        [Inject] private BallManager _ballManager;

        public override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("Extra ball!");
            _ballManager.CreateBonusBall.Execute(paddle.InitialBallPosTrfm.position);
        }
    }
}