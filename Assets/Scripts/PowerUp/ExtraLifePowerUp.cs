using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraLifePowerUp : PowerUpPresenter
    {
        public override void ApplyEffect(PowerUpStateMachine fsm)
        {
            UnityEngine.Debug.Log("Extra life!");
            //game.NumLives.Value += 1;
            //target.GetComponent<LifeCounter>().Value += 1;
        }
    }
}
