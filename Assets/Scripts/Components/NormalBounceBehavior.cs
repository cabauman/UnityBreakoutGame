using UnityEngine;

namespace BreakoutGame
{
    public sealed class NormalBounceBehavior : MonoBehaviour
    {
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Power>(out var power))
            {
                return;
            }

            if (_health != null)
            {
                var planeHealthValue = _health.Current;
                _health.Reduce(power.Value);
                if (power.Value > planeHealthValue)
                {
                    return;
                }
            }

            if (!collision.gameObject.TryGetComponent<Ball>(out var ball))
            {
                return;
            }

            var contactPoint = collision.GetContact(0);
            var planeNormal = -contactPoint.normal;
            var ballRb = collision.rigidbody;
            //Debug.DrawRay(contactPoint.point, planeNormal, Color.red, 2f);
            if (Vector2.Dot(ballRb.linearVelocity, planeNormal) < 0f)
            {
                ballRb.linearVelocity = Vector2.Reflect(
                    ballRb.linearVelocity,
                    planeNormal);
            }
        }
    }

    // Or ReflectBounceStrategy
    public sealed class NormalBounceStrategy : ICollisionStrategy
    {
        public void Resolve(Collision2D collision)
        {
            var other = collision.otherCollider.attachedRigidbody;
            other.linearVelocity = Vector2.Reflect(
                other.linearVelocity,
                collision.GetContact(0).normal);
        }
    }
}
