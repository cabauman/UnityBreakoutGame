using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PowerUp : MonoBehaviour
    {
        private PowerUpData _data;

        private void Start()
        {
            this
                .OnTriggerEnter2DAsObservable()
                .Subscribe(ApplyAndDestroy)
                .AddTo(this);
        }

        private void ApplyAndDestroy(Collider2D collider)
        {
            if (collider.CompareTag(Tags.DEAD_ZONE))
            {
                Destroy(gameObject);
                return;
            }

            if (collider.TryGetComponent<Paddle>(out var paddle))
            {
                _data.Action.ApplyEffect(paddle);
                Destroy(gameObject);
            }
        }

        public void Init(PowerUpData data)
        {
            _data = data;
        }
    }
}