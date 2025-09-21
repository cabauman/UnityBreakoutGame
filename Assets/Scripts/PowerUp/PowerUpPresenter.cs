using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public class PowerUpPresenter : MonoBehaviour
    {
        public PowerUp PowerUp { get; set; }

        private void Start()
        {
            if (PowerUp.Sprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = PowerUp.Sprite;
            }

            this
                .OnTriggerEnter2DAsObservable()
                .Subscribe(ApplyAndDestroy)
                .AddTo(this);

            this
                .OnTriggerEnter2DAsObservable()
                .Where(collider => collider.CompareTag(Tags.DEAD_ZONE))
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);
        }

        private void ApplyAndDestroy(Collider2D collider)
        {
            if (!collider.TryGetComponent<PaddlePresenter>(out var paddle))
            {
                return;
            }

            PowerUp.ApplyEffect(paddle);
            Destroy(gameObject);
        }
    }
}