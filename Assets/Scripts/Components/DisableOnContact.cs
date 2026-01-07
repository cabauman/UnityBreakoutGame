using UnityEngine;

namespace BreakoutGame
{
    public sealed class DisableOnContact : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Only disable objects that are moving downwards
            if (collider.attachedRigidbody.linearVelocityY >= 0f)
            {
                return;
            }

            if (!collider.CompareTag("Player"))
            {
                collider.gameObject.SetActive(false);
            }
        }
    }
}
