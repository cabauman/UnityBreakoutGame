using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class DecrementLifeOnContact : MonoBehaviour
    {
        [SerializeField]
        private LifeTracker _lifeTracker;

        private void Awake()
        {
            Ensure.NotNull(_lifeTracker);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                _lifeTracker.RemoveLife();
            }
        }
    }
}
