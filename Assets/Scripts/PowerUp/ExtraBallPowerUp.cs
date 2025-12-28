using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraBallPowerUp : PowerUpPresenter
    {
        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("Extra ball!");

            var ballManager = FindAnyObjectByType<BallManager>();
            Ensure.NotNull(ballManager);

            if (ballManager.Balls.Count == 0)
            {
                return;
            }

            var mainBall = ballManager.Balls[0];
            var ballSpawnData = new BallSpawnData
            {
                Position = mainBall.transform.position,
                Parent = null,
            };

            ballManager.SpawnBallCmd.Execute(ballSpawnData);

            var ballLauncher = go.GetComponent<BallLauncher>();
            if (ballLauncher != null && ballLauncher.IsAttached(mainBall.transform))
            {
                ballLauncher.Launch();
            }

            var bonusBall = ballManager.Balls[ballManager.Balls.Count - 1];
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
