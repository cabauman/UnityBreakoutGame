using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class PowerUpStateMachine : MonoBehaviour, IPostInject
    {
        [SerializeField]
        private Transform _ballParent;

        [Inject]
        private IReadOnlyLevelEvents _levelEvents;

        private readonly ReactiveProperty<IPowerUpState> _currentState = new();

        private ICollisionStrategy _defaultCollisionStrategy;

        public Transform BallParent => _ballParent;

        public ReadOnlyReactiveProperty<IPowerUpState> CurrentState => _currentState;

        private ICollisionStrategy CollisionStrategyOverride => _currentState.Value as ICollisionStrategy;

        public ICollisionStrategy CollisionStrategy => CollisionStrategyOverride == null
            ? _defaultCollisionStrategy
            : CollisionStrategyOverride;

        private void Awake()
        {
            ULog.Trace("");
            _defaultCollisionStrategy = GetComponentInChildren<ICollisionStrategy>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ULog.Trace(collision.gameObject.name);
            CollisionStrategy.Resolve(collision);
        }

        void IPostInject.PostInject()
        {
            ULog.Trace("");
            _levelEvents.LevelReady.Subscribe(isReady =>
            {
                if (!isReady)
                {
                    ClearMode();
                }
            })
            .AddTo(this);
        }

        public void Transition(IPowerUpState state)
        {
            ULog.Trace("");
            Ensure.NotNull(state);
            _currentState.Value?.Exit();
            _currentState.Value = state;
            _currentState.Value.Enter();
        }

        public void ClearMode()
        {
            ULog.Trace("");
            _currentState.Value?.Exit();
            _currentState.Value = null;
        }
    }
}
