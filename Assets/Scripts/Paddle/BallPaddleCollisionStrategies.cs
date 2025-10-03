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

    /// <summary>
    /// Example strategy using the generalized PlaneBouncingUtility for horizontal paddle bouncing.
    /// This demonstrates how to use the utility for the same behavior as NormalBounceStrategy.
    /// </summary>
    public class GeneralizedBounceStrategy : IBallPaddleCollisionStrategy
    {
        private readonly float _maxBounceAngleRad = 75f * Mathf.Deg2Rad;
        private readonly float _bounceForceMag = 1f;

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            // Using the utility for horizontal plane bouncing
            var paddleCenter = (Vector2)paddle.GraphicTrfm.position;
            var bounceForce = PlaneBouncingUtility.CalculateHorizontalPlaneBounce(
                paddleCenter,
                paddle.Width,
                point,
                _maxBounceAngleRad,
                _bounceForceMag
            );
            
            ball.SetForce(bounceForce);
        }
    }

    /// <summary>
    /// Example strategy demonstrating bouncing off an angled plane using the utility.
    /// This shows how to handle arbitrarily oriented planes (e.g., 45-degree angled paddle).
    /// </summary>
    public class AngledPlaneBounceStrategy : IBallPaddleCollisionStrategy
    {
        private readonly float _maxBounceAngleRad = 60f * Mathf.Deg2Rad;
        private readonly float _bounceForceMag = 1f;
        private readonly float _planeAngleDeg = 45f; // Angle of the plane in degrees

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var paddleCenter = (Vector2)paddle.GraphicTrfm.position;
            
            // Calculate the plane's normal and right vectors based on the plane angle
            float angleRad = _planeAngleDeg * Mathf.Deg2Rad;
            Vector2 planeNormal = new Vector2(-Mathf.Sin(angleRad), Mathf.Cos(angleRad));
            Vector2 planeRight = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            
            var bounceForce = PlaneBouncingUtility.CalculateBounceDirection(
                paddleCenter,
                planeNormal,
                planeRight,
                paddle.Width,
                point,
                _maxBounceAngleRad,
                _bounceForceMag
            );
            
            ball.SetForce(bounceForce);
        }
    }

    /// <summary>
    /// Example strategy that demonstrates using the utility with a rotating paddle.
    /// The plane orientation is determined by the paddle's current rotation.
    /// </summary>
    public class RotatingPlaneBounceStrategy : IBallPaddleCollisionStrategy
    {
        private readonly float _maxBounceAngleRad = 75f * Mathf.Deg2Rad;
        private readonly float _bounceForceMag = 1f;

        public void HandleCollision(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var paddleCenter = (Vector2)paddle.GraphicTrfm.position;
            
            // Get the paddle's current rotation to determine plane orientation
            float paddleRotationRad = paddle.GraphicTrfm.rotation.eulerAngles.z * Mathf.Deg2Rad;
            
            // Calculate plane normal and right vector based on paddle rotation
            Vector2 planeNormal = new Vector2(-Mathf.Sin(paddleRotationRad), Mathf.Cos(paddleRotationRad));
            Vector2 planeRight = new Vector2(Mathf.Cos(paddleRotationRad), Mathf.Sin(paddleRotationRad));
            
            var bounceForce = PlaneBouncingUtility.CalculateBounceDirection(
                paddleCenter,
                planeNormal,
                planeRight,
                paddle.Width,
                point,
                _maxBounceAngleRad,
                _bounceForceMag
            );
            
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
            paddle.AttachBall(ball);
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
            Vector2 planeRight,
            float planeWidth,
            Vector2 contactPoint,
            float maxBounceAngle = 75f * Mathf.Deg2Rad,
            float forceMagnitude = 1f)
        {
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
        /// Overload that automatically calculates the plane's right vector from its normal.
        /// Assumes the plane normal points "up" relative to the desired bounce behavior.
        /// </summary>
        public static Vector2 CalculateBounceDirection(
            Vector2 planeCenter,
            Vector2 planeNormal,
            float planeWidth,
            Vector2 contactPoint,
            float maxBounceAngle = 75f * Mathf.Deg2Rad,
            float forceMagnitude = 1f)
        {
            // Calculate right vector perpendicular to normal (90-degree counter-clockwise rotation)
            Vector2 planeRight = new Vector2(-planeNormal.y, planeNormal.x);
            
            return CalculateBounceDirection(
                planeCenter, 
                planeNormal, 
                planeRight, 
                planeWidth, 
                contactPoint, 
                maxBounceAngle, 
                forceMagnitude);
        }
        
        /// <summary>
        /// Simplified overload for horizontal planes (like the original paddle behavior).
        /// </summary>
        public static Vector2 CalculateHorizontalPlaneBounce(
            Vector2 planeCenter,
            float planeWidth,
            Vector2 contactPoint,
            float maxBounceAngle = 75f * Mathf.Deg2Rad,
            float forceMagnitude = 1f)
        {
            Vector2 planeNormal = Vector2.up;
            return CalculateBounceDirection(
                planeCenter, 
                planeNormal, 
                planeWidth, 
                contactPoint, 
                maxBounceAngle, 
                forceMagnitude);
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
        
        /// <summary>
        /// Gets the signed distance of a point from the center of a plane along its width.
        /// </summary>
        /// <param name="planeCenter">Center of the plane</param>
        /// <param name="planeRight">Right vector of the plane</param>
        /// <param name="point">Point to measure</param>
        /// <returns>Signed distance along the plane's width (-width/2 to +width/2)</returns>
        public static float GetSignedDistanceFromPlaneCenter(Vector2 planeCenter, Vector2 planeRight, Vector2 point)
        {
            Vector2 offset = point - planeCenter;
            return Vector2.Dot(offset, planeRight);
        }
        
        /// <summary>
        /// Normalizes a signed distance to the (-1, 1) range based on plane width.
        /// </summary>
        /// <param name="signedDistance">Raw signed distance from plane center</param>
        /// <param name="planeWidth">Total width of the plane</param>
        /// <returns>Normalized distance in (-1, 1) range</returns>
        public static float NormalizeDistanceToPlaneWidth(float signedDistance, float planeWidth)
        {
            return Mathf.Clamp(signedDistance / (planeWidth * 0.5f), -1f, 1f);
        }
        
        /// <summary>
        /// Projects a 2D vector onto a plane defined by its normal vector.
        /// This is the 2D equivalent of Vector3.ProjectOnPlane.
        /// </summary>
        /// <param name="vector">The vector to project</param>
        /// <param name="planeNormal">The normal vector of the plane (should be normalized)</param>
        /// <returns>The projected vector that lies on the plane</returns>
        public static Vector2 ProjectOnPlane(Vector2 vector, Vector2 planeNormal)
        {
            // Project vector onto normal: proj = (v · n) * n
            float dotProduct = Vector2.Dot(vector, planeNormal);
            Vector2 projection = dotProduct * planeNormal;
            
            // Subtract the projection from the original vector to get the component on the plane
            return vector - projection;
        }
    }
}
