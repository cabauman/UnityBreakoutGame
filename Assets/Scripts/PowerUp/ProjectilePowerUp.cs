using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class ProjectilePowerUp : PowerUpPresenter//, IPowerUp
    {
        public void Apply(GameObject target)
        {
            var state = new ProjectileState();
            target.GetComponent<PowerUpStateMachine>().Transition(state);
        }

        public override void ApplyEffect(PowerUpStateMachine fsm)
        {
            var state = new ProjectileState();
            fsm.Transition(state);
        }
    }

    public sealed class ProjectileState : IPowerUpState, ICollisionStrategy
    {
        private readonly ICollisionStrategy _strategy;

        public ProjectileState()
        {
            _strategy = ContactPointBounceStrategy.Instance;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Execute(Collision2D collision)
        {
            _strategy.Execute(collision);
            // TODO: spawn a projectile that moves in the direction of the normal
        }
    }
}
