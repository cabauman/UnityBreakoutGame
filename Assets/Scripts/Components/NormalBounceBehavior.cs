using UnityEngine;

namespace BreakoutGame
{
    public sealed class NormalBounceBehavior : MonoBehaviour, ICollisionStrategy
    {
        //private void OnTriggerEnter2D(Collider2D collider)
        //{
        //    Debug.Log("OnTriggerEnter2D");
        //    var other = collider.attachedRigidbody;
        //    var contactPoints = new ContactPoint2D[1];
        //    collider.GetContacts(contactPoints);
        //    other.linearVelocity = Vector2.Reflect(
        //        other.linearVelocity,
        //        contactPoints[0].normal);
        //}
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
    // Or ReflectBounceStrategy
    public sealed class NormalBounceStrategy : ICollisionStrategy
    {
        public void Execute(Collision2D collision)
        {
            var other = collision.otherCollider.attachedRigidbody;
            other.linearVelocity = Vector2.Reflect(
                other.linearVelocity,
                collision.GetContact(0).normal);
        }
    }
}
