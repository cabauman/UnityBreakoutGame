using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraLifePowerUp : PowerUpPresenter
    {
        public override void ApplyEffect(Game game, Vector3 position)
        {
            UnityEngine.Debug.Log("Extra life!");
            game.NumLives.Value += 1;
        }
    }
}
