using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraLifePowerUpAction : PowerUpAction
    {
        private readonly Game2 _game;
        public ExtraLifePowerUpAction(Game2 game)
        {
            _game = game;
        }
        public override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("Extra life!");
            _game.NumLives.Value += 1;
        }
    }
}