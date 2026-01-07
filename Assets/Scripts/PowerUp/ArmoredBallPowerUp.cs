using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ArmoredBallPowerUp : PowerUp
    {
        [Inject]
        private BallManager _ballManager;

        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("");

            if (!go.transform.parent.TryGetComponent<PowerUpStateMachine>(out var fsm))
            {
                return;
            }

            var state = new ArmoredBallState(_ballManager);
            fsm.Transition(state);
        }
    }

    public sealed class ArmoredBallState : IPowerUpState
    {
        private readonly BallManager _ballManager;

        private int _previousPower;

        public ArmoredBallState(BallManager ballManager)
        {
            Ensure.NotNull(ballManager);
            _ballManager = ballManager;
        }

        public string Name => "Armored Ball";

        public void Enter()
        {
            ULog.Trace("");

            if (_ballManager.Balls.Count == 0)
            {
                return;
            }

            var mainBall = _ballManager.Balls[0];
            _previousPower = mainBall.Power;
            mainBall.Power = 100;
            ULog.Trace($"Ball power set to 100 from {_previousPower}");
        }

        public void Exit()
        {
            ULog.Trace("");

            if (_ballManager.Balls.Count == 0)
            {
                return;
            }

            var mainBall = _ballManager.Balls[0];
            mainBall.Power = _previousPower;
            ULog.Trace($"Ball power restored to {_previousPower}");
        }
    }
}
