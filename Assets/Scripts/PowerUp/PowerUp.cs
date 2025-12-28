using GameCtor.DevToolbox;
using R3;
using R3.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public abstract class PowerUpPresenter : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;

        private void Start()
        {
            Ensure.NotNull(_sprite);

            GetComponent<SpriteRenderer>().sprite = _sprite;
            var tag = TagHandle.GetExistingTag("Player");
            this
                .OnTriggerEnter2DAsObservable()
                .Where(tag, static (collider, tag) => collider.CompareTag(tag))
                .Subscribe(this, static (collider, @this) => ApplyAndDestroy(@this, collider))
                .AddTo(this);
        }

        public abstract void ApplyEffect(GameObject go);

        private static void ApplyAndDestroy(PowerUpPresenter @this, Collider2D collider)
        {
            // TODO: Replace this with a bool getter
            var ballManager = FindAnyObjectByType<BallManager>();
            Ensure.NotNull(ballManager);

            if (ballManager.Balls.Count > 0)
            {
                @this.ApplyEffect(collider.gameObject);
            }

            Destroy(@this.gameObject);
        }
    }
}
