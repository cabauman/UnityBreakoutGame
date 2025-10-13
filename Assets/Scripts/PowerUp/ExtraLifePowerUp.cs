using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraLifePowerUpAction : PowerUp
    {
        [Inject] private Game _game;

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("Extra life!");
            //_game.NumLives.Value += 1;
        }
    }
}