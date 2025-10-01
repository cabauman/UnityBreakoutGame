using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public abstract class PowerUp : MonoBehaviour
    {
        private void Awake()
        {
            this
                .OnTriggerEnter2DAsObservable()
                .Subscribe(ApplyAndDestroy)
                .AddTo(this);
        }

        protected abstract void ApplyEffect(Paddle paddle);

        private void ApplyAndDestroy(Collider2D collider)
        {
            if (collider.CompareTag(Tags.DEAD_ZONE))
            {
                Destroy(gameObject);
                return;
            }

            if (collider.TryGetComponent<Paddle>(out var paddle))
            {
                ApplyEffect(paddle);
                Destroy(gameObject);
            }
        }
    }
}