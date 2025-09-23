using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraBallPowerUpAction : PowerUpAction
    {
        private readonly BallManager _ballManager;
        public ExtraBallPowerUpAction(BallManager ballManager)
        {
            _ballManager = ballManager;
        }
        public override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("Extra ball!");
            //_ballManager.CreateBonusBall.Execute(paddle.InitialBallPosTrfm.position);
        }
    }
}