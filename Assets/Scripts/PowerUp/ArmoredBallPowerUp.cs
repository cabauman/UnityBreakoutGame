using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    public class ArmoredBallPowerUp : PowerUpPresenter
    {
        public override void ApplyEffect(GameObject go)
        {
            if (!go.transform.parent.TryGetComponent<PowerUpStateMachine>(out var fsm))
            {
                return;
            }

            var state = new ArmoredBallState();
            fsm.Transition(state);
        }
    }

    public sealed class ArmoredBallState : IPowerUpState
    {
        private int _previousPower;

        public void Enter()
        {
            ULog.Trace("Armored Ball Power-Up Activated");
            var ballManager = GameObject.FindAnyObjectByType<BallManager>();
            Ensure.NotNull(ballManager);

            if (ballManager.Balls.Count == 0)
            {
                return;
            }

            var mainBall = ballManager.Balls[0];
            _previousPower = mainBall.Power;
            mainBall.Power = 100;
            ULog.Trace($"Armored Ball Power-Up Applied: Ball power set to 100 from {_previousPower}");
        }

        public void Exit()
        {
            ULog.Trace("Armored Ball Power-Up Deactivated");
            var ballManager = GameObject.FindAnyObjectByType<BallManager>();
            Ensure.NotNull(ballManager);

            if (ballManager.Balls.Count == 0)
            {
                return;
            }

            var mainBall = ballManager.Balls[0];
            mainBall.Power = _previousPower;
            ULog.Trace($"Armored Ball Power-Up Removed: Ball power restored to {_previousPower}");
        }
    }
}
