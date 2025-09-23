namespace BreakoutGame
{
    public interface IBallPaddleCollisionStrategy
    {
        void HandleCollision(BallPresenter ball, PaddlePresenter paddle);
    }

    public class NormalBounceStrategy : IBallPaddleCollisionStrategy
    {
        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle)
        {
            //var localContact = collision.transform.InverseTransformPoint(collision.contacts[0].point);
            //var paddleWidth = collision.collider.GetComponent<SpriteRenderer>().bounds.size.x;

            //// Map the horizontal contact point to the (0, 1) range.
            //// Input is in the range (-paddleWidth/2, paddleWidth/2)
            //var normalizedLocalContactX = localContact.x / paddleWidth + 0.5f;
            //var bounceAngle = Mathf.Lerp(
            //    Mathf.PI / 2 + _maxPaddleBounceAngleInRadians,
            //    Mathf.PI / 2 - _maxPaddleBounceAngleInRadians,
            //    normalizedLocalContactX
            //);

            //var bounceForce = new Vector2
            //{
            //    x = Mathf.Cos(bounceAngle) * Ball.InitialForce,
            //    y = Mathf.Sin(bounceAngle) * Ball.InitialForce
            //};

            //_rigidbody.linearVelocity = Vector2.zero;
            //_rigidbody.AddForce(bounceForce);
        }
    }

    public class ReverseBounceStrategy : IBallPaddleCollisionStrategy
    {
        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle)
        {
            //ball.ReverseBounce();
        }
    }

    public class MagnetBounceStrategy : IBallPaddleCollisionStrategy
    {
        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle)
        {
            //ball.StickToPaddle(paddle);
        }
    }
}
