using UnityEngine;

namespace BreakoutGame
{
    public class ExtraLifePowerUp : PowerUp
    {
        public override string SpriteName { get; } = "ExtraLifePowerUp";

        public override void ApplyEffect(Game game, Vector3 position)
        {
            Debug.Log("Extra life!");
            game.NumLives.Value += 1;
        }
    }
}