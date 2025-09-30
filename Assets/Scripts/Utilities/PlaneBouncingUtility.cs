using UnityEngine;

namespace BreakoutGame.Utilities
{
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
    }
}