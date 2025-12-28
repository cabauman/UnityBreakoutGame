using UnityEngine;

namespace BreakoutGame
{
    public sealed class PowerUpStateMachine : MonoBehaviour
    {
        [SerializeField]
        private Transform _ballParent;

        private IPowerUpState _currentState;
        private ICollisionStrategy _defaultCollisionStrategy;

        public Transform BallParent => _ballParent;

        private ICollisionStrategy CollisionStrategyOverride => _currentState as ICollisionStrategy;
        public ICollisionStrategy CollisionStrategy => CollisionStrategyOverride == null
            ? _defaultCollisionStrategy
            : CollisionStrategyOverride;

        private void Awake()
        {
            //_defaultCollisionStrategy = ContactPointBounceStrategy.Instance;
            _defaultCollisionStrategy = GetComponentInChildren<ICollisionStrategy>();
        }

        public void Transition(IPowerUpState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public void ClearMode()
        {
            _currentState?.Exit();
            _currentState = null;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionStrategy.Resolve(collision);
        }
    }
}
