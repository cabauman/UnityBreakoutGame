using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Pool;

namespace BreakoutGame
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public abstract partial class PowerUp : MonoBehaviour, IPostInject
    {
        [Inject]
        private IReadOnlyLevelEvents _levelEvents;

        public ObjectPool<PowerUp> Pool { get; set; }

        private void Awake()
        {
            ULog.Trace("");
            var tag = TagHandle.GetExistingTag("Player");
            this
                .OnTriggerEnter2DAsObservable()
                .Where(tag, static (collider, tag) => collider.CompareTag(tag))
                .Subscribe(this, static (collider, @this) => ApplyAndDestroy(@this, collider))
                .AddTo(this);
        }

        private void OnDisable()
        {
            ULog.Trace("");
            Ensure.NotNull(Pool);
            Pool.Release(this);
        }

        void IPostInject.PostInject()
        {
            _levelEvents.LevelReady.Subscribe(isReady =>
            {
                if (!isReady && this != null)
                {
                    gameObject.SetActive(isReady);
                }
            });
        }

        public abstract void ApplyEffect(GameObject go);

        private static void ApplyAndDestroy(PowerUp @this, Collider2D collider)
        {
            if (@this._levelEvents.LevelReady.CurrentValue)
            {
                @this.ApplyEffect(collider.gameObject);
            }

            @this.gameObject.SetActive(false);
        }
    }
}
