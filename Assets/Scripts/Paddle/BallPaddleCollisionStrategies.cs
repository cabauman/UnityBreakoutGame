using UnityEngine;

namespace BreakoutGame
{
    public interface IBallPaddleCollisionStrategy
    {
        void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point);
    }

    public class NormalBounceStrategy : IBallPaddleCollisionStrategy
    {
        //private readonly float _maxBounceAngleDeg = 80f;
        private readonly float _maxBounceAngleRad = 75f * Mathf.Deg2Rad;
        private readonly float _bounceForceMag = 1f;

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var localContact = paddle.GraphicTrfm.InverseTransformPoint(point);

            // Map the horizontal contact point to the (0, 1) range.
            // Input is in the range (-paddleWidth/2, paddleWidth/2)
            var normalizedLocalContactX = localContact.x / paddle.Width + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + _maxBounceAngleRad,
                Mathf.PI / 2 - _maxBounceAngleRad,
                normalizedLocalContactX
            );

            var bounceForce = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle)) * _bounceForceMag;
            ball.SetForce(bounceForce);
        }
    }

    public class ReverseBounceStrategy : IBallPaddleCollisionStrategy
    {
        private readonly float _maxBounceAngleRad = 75f * Mathf.Deg2Rad;
        private readonly float _bounceForceMag = 1f;

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var localContact = paddle.GraphicTrfm.InverseTransformPoint(point);

            // Map the horizontal contact point to the (0, 1) range.
            // Input is in the range (-paddleWidth/2, paddleWidth/2)
            var normalizedLocalContactX = localContact.x / paddle.Width + 0.5f;
            Debug.Log($"Normalized contact X: {normalizedLocalContactX}");
            normalizedLocalContactX = 1 - normalizedLocalContactX;
            Debug.Log($"Reversed normalized contact X: {normalizedLocalContactX}");
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + _maxBounceAngleRad,
                Mathf.PI / 2 - _maxBounceAngleRad,
                normalizedLocalContactX
            );

            var bounceForce = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle)) * _bounceForceMag;
            ball.SetForce(bounceForce);
        }
    }

    public class MagnetBounceStrategy : IBallPaddleCollisionStrategy
    {
        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            ball.AttachTo(paddle.Trfm);
        }
    }
}
