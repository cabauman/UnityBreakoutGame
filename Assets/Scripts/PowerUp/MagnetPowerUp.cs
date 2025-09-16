using UnityEngine;

namespace BreakoutGame
{
    public class MagnetPowerUp : PowerUp
    {
        public override string SpriteName { get; } = "MagnetPowerUp";

        public override void ApplyEffect(Game game, Vector3 position)
        {
            Debug.Log("MagnetPowerUp");
        }
    }
}