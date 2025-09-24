using UnityEngine;

namespace BreakoutGame
{
    public interface IBallPaddleCollisionStrategy
    {
        void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point);
    }

    public class NormalBounceStrategy : IBallPaddleCollisionStrategy
    {
        private readonly float _maxPaddleBounceAngleInRadians;
        //_maxPaddleBounceAngleRad = config._maxPaddleBounceAngle* Mathf.Deg2Rad;

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var localContact = paddle.GraphicTrfm.InverseTransformPoint(point);

            // Map the horizontal contact point to the (0, 1) range.
            // Input is in the range (-paddleWidth/2, paddleWidth/2)
            var normalizedLocalContactX = localContact.x / paddle.Width + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + _maxPaddleBounceAngleInRadians,
                Mathf.PI / 2 - _maxPaddleBounceAngleInRadians,
                normalizedLocalContactX
            );

            var bounceForce = new Vector2
            {
                x = Mathf.Cos(bounceAngle) * ball.InitialForce,
                y = Mathf.Sin(bounceAngle) * ball.InitialForce
            };

            ball.SetForce(bounceForce);
        }
    }

    public class ReverseBounceStrategy : IBallPaddleCollisionStrategy
    {
        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            //ball.ReverseBounce();
        }
    }

    public class MagnetBounceStrategy : IBallPaddleCollisionStrategy
    {
        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            //ball.StickToPaddle(paddle);
        }
    }
}
