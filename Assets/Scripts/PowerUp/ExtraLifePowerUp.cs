using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraLifePowerUp : PowerUp
    {
        [Inject] Game2 _game;

        public override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("Extra life!");
            _game.NumLives.Value += 1;
        }
    }
}