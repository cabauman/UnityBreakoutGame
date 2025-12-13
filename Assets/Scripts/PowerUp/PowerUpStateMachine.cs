using UnityEngine;

namespace BreakoutGame
{
    public sealed class PowerUpStateMachine : MonoBehaviour
    {
        private IPowerUpState _currentState;
        private ICollisionStrategy _defaultCollisionStrategy;

        private ICollisionStrategy CollisionStrategyOverride => _currentState as ICollisionStrategy;
        public ICollisionStrategy CollisionStrategy => CollisionStrategyOverride == null
            ? _defaultCollisionStrategy
            : CollisionStrategyOverride;

        private void Awake()
        {
            _defaultCollisionStrategy = ContactPointBounceStrategy.Instance;
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
    }
}
