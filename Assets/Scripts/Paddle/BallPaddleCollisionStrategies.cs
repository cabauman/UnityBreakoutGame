using UnityEngine;
using UnityEngine.Assertions;

namespace BreakoutGame
{
    public interface IBallPaddleCollisionStrategy
    {
        void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point);
    }

    public sealed class ProjectileCollisionStrategyDecorator : IBallPaddleCollisionStrategy
    {
        private readonly IBallPaddleCollisionStrategy _baseStrategy;
        private readonly float _projectileForceMag = 1f;
        private readonly GameObject _projectilePrefab;

        public ProjectileCollisionStrategyDecorator(IBallPaddleCollisionStrategy baseStrategy, GameObject projectilePrefab)
        {
            _baseStrategy = baseStrategy;
            _projectilePrefab = projectilePrefab;
        }

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            _baseStrategy.HandleCollision(ball, paddle, point);

            // Launch a projectile upwards from the paddle's position
            var projectileInstance = GameObject.Instantiate(_projectilePrefab, point, Quaternion.identity);
            var projectileRigidbody = projectileInstance.GetComponent<Rigidbody2D>();
            Assert.IsNotNull(projectileRigidbody, "Projectile prefab must have a Rigidbody2D component.");
            projectileRigidbody.AddForce(Vector2.up * _projectileForceMag, ForceMode2D.Impulse);
        }
    }

    public class NormalBounceStrategy : IBallPaddleCollisionStrategy
    {
        //private readonly float _maxBounceAngleDeg = 80f;
        private readonly float _maxBounceAngleRad = 75f * Mathf.Deg2Rad;
        private readonly float _bounceForceMag = 1f;

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var paddleCenter = (Vector2)paddle.GraphicTrfm.position;
            var bounceForce = PlaneBouncingUtility.CalculateBounceDirection(
                paddleCenter,
                Vector2.up,
                paddle.Width,
                point,
                _maxBounceAngleRad,
                _bounceForceMag
            );
            ball.SetForce(bounceForce);
        }
    }

    public class ReverseBounceStrategy : IBallPaddleCollisionStrategy, IGameWorldEffect
    {
        private readonly float _maxBounceAngleRad = 75f * Mathf.Deg2Rad;
        private readonly float _bounceForceMag = 1f;

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var paddleCenter = (Vector2)paddle.GraphicTrfm.position;
            var bounceForce = PlaneBouncingUtility.CalculateBounceDirection(
                paddleCenter,
                Vector2.up,
                paddle.Width,
                point,
                _maxBounceAngleRad,
                _bounceForceMag
            );
            bounceForce.x = -bounceForce.x;
            ball.SetForce(bounceForce);
        }

        public void OnEnter(PaddlePresenter paddle)
        {
            paddle.SetBallCollisionStrategy(this);
        }

        public void OnExit(PaddlePresenter paddle)
        {
            paddle.ResetBallCollisionStrategy();
        }
    }

    public class MagnetBounceStrategy : IBallPaddleCollisionStrategy, IGameWorldEffect
    {
        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            paddle.AttachBall(ball);
        }

        public void OnEnter(PaddlePresenter paddle)
        {
            paddle.SetBallCollisionStrategy(this);
        }

        public void OnExit(PaddlePresenter paddle)
        {
            paddle.ResetBallCollisionStrategy();
        }
    }

    /// <summary>
    /// Utility class for calculating bounce directions off arbitrarily oriented planes.
    /// Generalizes the bounce calculation based on signed distance from plane center.
    /// </summary>
    public static class PlaneBouncingUtility
    {
        /// <summary>
        /// Calculates bounce direction based on contact point relative to plane center.
        /// </summary>
        /// <param name="planeCenter">World position of the plane center</param>
        /// <param name="planeNormal">Normal vector of the plane (should be normalized)</param>
        /// <param name="planeRight">Right vector along the plane surface (should be normalized)</param>
        /// <param name="planeWidth">Width of the plane along the right vector</param>
        /// <param name="contactPoint">World position where contact occurred</param>
        /// <param name="maxBounceAngle">Maximum bounce angle in radians from the normal</param>
        /// <param name="forceMagnitude">Magnitude of the resulting force vector</param>
        /// <returns>Normalized bounce direction vector</returns>
        public static Vector2 CalculateBounceDirection(
            Vector2 planeCenter,
            Vector2 planeNormal,
            float planeWidth,
            Vector2 contactPoint,
            float maxBounceAngle = 75f * Mathf.Deg2Rad,
            float forceMagnitude = 1f)
        {
            // Calculate right vector perpendicular to normal (90-degree counter-clockwise rotation)
            Vector2 planeRight = Vector2.right; //new Vector2(-planeNormal.y, planeNormal.x);

            // Project contact point onto the plane's right vector to get signed distance from center
            Vector2 contactOffset = contactPoint - planeCenter;
            float signedDistanceFromCenter = Vector2.Dot(contactOffset, planeRight);
            
            // Normalize the distance to (-1, 1) range based on plane width
            float normalizedDistance = Mathf.Clamp(signedDistanceFromCenter / (planeWidth * 0.5f), -1f, 1f);
            
            // Convert to (0, 1) range for angle interpolation
            float normalizedPosition = (normalizedDistance + 1f) * 0.5f;
            
            // Calculate bounce angle relative to the plane normal
            // When normalizedPosition = 0 (left edge), bounce to the right of normal
            // When normalizedPosition = 1 (right edge), bounce to the left of normal
            float bounceAngleFromNormal = Mathf.Lerp(maxBounceAngle, -maxBounceAngle, normalizedPosition);
            
            // Calculate the bounce direction by rotating the normal by the bounce angle
            Vector2 bounceDirection = RotateVector2D(planeNormal, bounceAngleFromNormal);
            
            return bounceDirection * forceMagnitude;
        }
        
        /// <summary>
        /// Rotates a 2D vector by the specified angle in radians.
        /// </summary>
        private static Vector2 RotateVector2D(Vector2 vector, float angleRadians)
        {
            float cos = Mathf.Cos(angleRadians);
            float sin = Mathf.Sin(angleRadians);
            
            return new Vector2(
                vector.x * cos - vector.y * sin,
                vector.x * sin + vector.y * cos
            );
        }
    }
}
