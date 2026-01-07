using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraBallPowerUp : PowerUp
    {
        [Inject]
        private BallManager _ballManager;

        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("");

            Ensure.NotNull(_ballManager);

            if (_ballManager.Balls.Count == 0)
            {
                return;
            }

            var mainBall = _ballManager.Balls[0];
            var ballSpawnData = new BallSpawnData
            {
                Position = mainBall.transform.position,
                Parent = null,
            };

            _ballManager.SpawnBallCmd.Execute(ballSpawnData);

            var ballLauncher = go.transform.parent.GetComponent<BallLauncher>();
            if (ballLauncher != null && ballLauncher.IsAttached(mainBall.transform))
            {
                ballLauncher.Launch();
            }

            var bonusBall = _ballManager.Balls[_ballManager.Balls.Count - 1];
            var mainVelocity = mainBall.GetComponent<Rigidbody2D>().linearVelocity;
            var angleOffsetRad = Mathf.Deg2Rad * 10f;
            var cosAngle = Mathf.Cos(angleOffsetRad);
            var sinAngle = Mathf.Sin(angleOffsetRad);
            var rotatedVelocity = new Vector2(
                mainVelocity.x * cosAngle - mainVelocity.y * sinAngle,
                mainVelocity.x * sinAngle + mainVelocity.y * cosAngle);

            bonusBall.GetComponent<Rigidbody2D>().linearVelocity = rotatedVelocity;
        }
    }
}
