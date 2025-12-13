using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraBallPowerUp : PowerUpPresenter
    {
        public override void ApplyEffect(PowerUpStateMachine fsm)
        {
            UnityEngine.Debug.Log("Extra ball!");
            //game.CreateBonusBall.Execute(new Ball(50, 1, position));
        }
    }
}
