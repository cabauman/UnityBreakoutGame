using UnityEngine;

namespace BreakoutGame
{
    public sealed class ContactPointBounceBehavior : MonoBehaviour, ICollisionStrategy
    {
        [SerializeField]
        private float _maxBounceAngleRad = 1.30899692f; // 75 degrees

        public void Resolve(Collision2D collision)
        {
            var ball = collision.rigidbody;
            var contactPoint = collision.GetContact(0);

            var planeCenter = transform.position;
            var planeWidth = GetComponent<Collider2D>().bounds.size.x;
            var bounceDirection = PlaneBouncingUtility.CalculateBounceDirection(
                planeCenter,
                planeNormal: -contactPoint.normal,
                planeWidth,
                contactPoint.point,
                _maxBounceAngleRad);

            ball.linearVelocity = bounceDirection * ball.linearVelocity.magnitude;
        }
    }
    // Or WeightedBounceStrategy
    public sealed class ContactPointBounceStrategy : ICollisionStrategy
    {
        public static readonly ContactPointBounceStrategy Instance = new();

        private readonly float _maxBounceAngleRad;

        private ContactPointBounceStrategy(float maxBounceAngleRad = 1.30899692f)
        {
            _maxBounceAngleRad = maxBounceAngleRad;
        }

        public void Resolve(Collision2D collision)
        {
            var ball = collision.rigidbody;
            var contactPoint = collision.GetContact(0);

            var planeCollider = collision.otherCollider;
            var planeCenter = planeCollider.transform.position;
            var planeWidth = planeCollider.GetComponent<Collider2D>().bounds.size.x;
            var bounceDirection = PlaneBouncingUtility.CalculateBounceDirection(
                planeCenter,
                planeNormal: -contactPoint.normal,
                planeWidth,
                contactPoint.point,
                _maxBounceAngleRad);

            ball.linearVelocity = bounceDirection * ball.linearVelocity.magnitude;
        }
    }
}
