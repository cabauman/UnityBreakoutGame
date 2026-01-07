using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddleSizePowerUp : PowerUp
    {
        [SerializeField]
        private float _widthMultiplier = 1.4f;

        [SerializeField]
        private float _effectDuration = 5f;

        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("");

            if (!go.transform.parent.TryGetComponent<PowerUpStateMachine>(out var fsm))
            {
                return;
            }

            var renderer = go.GetComponentInChildren<SpriteRenderer>();
            var state = new PaddleSizeState(renderer.transform, _widthMultiplier);
            fsm.Transition(state);
        }
    }

    public sealed class PaddleSizeState : IPowerUpState
    {
        private readonly Transform _transform;
        private readonly float _widthMultiplier;

        public PaddleSizeState(Transform transform, float widthMultiplier)
        {
            _transform = transform;
            _widthMultiplier = widthMultiplier;
        }

        public string Name => "Grow";

        public void Enter()
        {
            ULog.Trace("");
            var scale = _transform.localScale;
            scale.x *= _widthMultiplier;
            _transform.localScale = scale;
        }

        public void Exit()
        {
            ULog.Trace("");
            var scale = _transform.localScale;
            scale.x /= _widthMultiplier;
            _transform.localScale = scale;
        }
    }
}
