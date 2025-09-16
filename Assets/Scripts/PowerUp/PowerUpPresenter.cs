using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public class PowerUpPresenter : MonoBehaviour
    {
        private void Start()
        {
            var sprite = Resources.Load<Sprite>(string.Format("Sprites/{0}", PowerUp.SpriteName));
            if (sprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = sprite;
            }

            this
                .OnTriggerEnter2DAsObservable()
                .Select(collider => collider.transform.parent.GetComponent<PaddlePresenter>())
                .Where(x => x != null)
                .Subscribe(_ => ApplyAndDestroy())
                .AddTo(this);

            this
                .OnTriggerEnter2DAsObservable()
                .Where(collider => collider.CompareTag(Tags.DEAD_ZONE))
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);
        }

        public PowerUp PowerUp { get; set; }

        private void ApplyAndDestroy()
        {
            var gamePresenter = FindAnyObjectByType<GamePresenter>();
            PowerUp.ApplyEffect(gamePresenter.Game, transform.position);
            Destroy(gameObject);
        }
    }
}