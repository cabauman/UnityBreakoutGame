using UnityEngine;

namespace BreakoutGame
{
    public class ArmoredBallPowerUp : PowerUpPresenter
    {
        public void Apply(GameObject target)
        {
            var state = new ArmoredBallState();
            target.GetComponent<PowerUpStateMachine>().Transition(state);
        }

        public override void ApplyEffect(PowerUpStateMachine fsm)
        {
            var state = new ArmoredBallState();
            fsm.Transition(state);
        }
    }

    public sealed class ArmoredBallState : IPowerUpState
    {
        public void Enter()
        {
            // set all destructible objects (bricks) to ignore collisions with the ball (NullCollisionStrategy)
            // set ball's DamageOnContact to very high value to ensure it can destroy any brick it touches
            // Either access BrickManager and BallManager OR do a global search for DamageOnContact and ICollisionStrategy
            // (or a tag or something) to identify bricks and balls indirectly.

            // if (collision.otherCollider.TryGetComponent<DamageOnContact>(out var component))
            // {
            //     component.SetDamageOverride(1_000);
            // }
            // if (collision.otherCollider.TryGetComponent<Health>(out var health))
            // {
            //     health.Reduce(1_000);
            // }
        }

        public void Exit()
        {
            // NEXT: revert damage override and all destructible objects (bricks) to normal collisions with the ball
        }
    }
}
