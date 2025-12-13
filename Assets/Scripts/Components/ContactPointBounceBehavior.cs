using UnityEngine;

namespace BreakoutGame
{
    public sealed class ContactPointBounceBehavior : MonoBehaviour, ICollisionStrategy
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var other = collision.rigidbody;
            var contactPoint = collision.GetContact(0);
            other.linearVelocity = Vector2.Reflect(
                other.linearVelocity,
                contactPoint.normal);
        }
        public void Execute(Collision2D collision)
        {
            var other = collision.otherCollider.attachedRigidbody;
            other.linearVelocity = Vector2.Reflect(
                other.linearVelocity,
                collision.GetContact(0).normal);
        }
    }
    // Or WeightedBounceStrategy
    public sealed class ContactPointBounceStrategy : ICollisionStrategy
    {
        public static readonly ContactPointBounceStrategy Instance = new();

        private ContactPointBounceStrategy() { }

        public void Execute(Collision2D collision)
        {
            var other = collision.otherCollider.attachedRigidbody;
            other.linearVelocity = Vector2.Reflect(
                other.linearVelocity,
                collision.GetContact(0).normal);
        }
    }
}
