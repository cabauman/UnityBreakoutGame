using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraLifePowerUp : PowerUpPresenter
    {
        [Inject]
        private Game2 _game;

        protected override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("Extra life!");
            _game.NumLives.Value += 1;
        }
    }
}