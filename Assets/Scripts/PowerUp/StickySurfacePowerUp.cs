using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class StickySurfacePowerUp : PowerUpPresenter
    {
        public override void ApplyEffect(GameObject go)
        {
            if (!go.transform.parent.TryGetComponent<PowerUpStateMachine>(out var fsm))
            {
                return;
            }

            var state = new StickySurfaceState(fsm.BallParent);
            fsm.Transition(state);
        }
    }

    public sealed class StickySurfaceState : IPowerUpState, ICollisionStrategy
    {
        private readonly Transform _ballParent;

        public StickySurfaceState(Transform ballParent)
        {
            Ensure.NotNull(ballParent);
            _ballParent = ballParent;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Resolve(Collision2D collision)
        {
            collision.rigidbody.linearVelocity = Vector2.zero;
            collision.transform.SetParent(_ballParent);
            var pos = collision.transform.localPosition;
            pos.y = 0;
            collision.transform.localPosition = pos;
        }
    }
}
