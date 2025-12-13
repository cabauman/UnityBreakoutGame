using GameCtor.DevToolbox;
using R3;
using R3.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public abstract class PowerUpPresenter : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;

        private void Start()
        {
            Ensure.NotNull(_sprite);

            GetComponent<SpriteRenderer>().sprite = _sprite;

            this
                .OnTriggerEnter2DAsObservable()
                .Select(collider => collider.transform.parent.GetComponentInParent<PowerUpStateMachine>())
                .Where(x => x != null)
                .Subscribe(fsm => ApplyAndDestroy(fsm))
                .AddTo(this);

            this
                .OnTriggerEnter2DAsObservable()
                .Where(collider => collider.CompareTag(Tags.DeadZone))
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);
        }

        public abstract void ApplyEffect(PowerUpStateMachine fsm);

        private void ApplyAndDestroy(PowerUpStateMachine fsm)
        {
            ApplyEffect(fsm);
            Destroy(gameObject);
        }
    }
}
