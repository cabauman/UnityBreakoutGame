using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class StickySurfacePowerUp : PowerUpPresenter//, IPowerUp
    {
        public void Apply(GameObject target)
        {
            var state = new StickySurfaceState();
            target.GetComponent<PowerUpStateMachine>().Transition(state);
        }

        public override void ApplyEffect(PowerUpStateMachine fsm)
        {
            var state = new StickySurfaceState();
            fsm.Transition(state);
        }
    }

    public sealed class StickySurfaceState : IPowerUpState, ICollisionStrategy
    {
        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Execute(Collision2D collision)
        {
            // TODO: Set the other object to be a child of this object to simulate "sticking"
            // The object will be accessible from the launcher component of the paddle
        }
    }
}
