using UnityEngine;

namespace BreakoutGame
{
    public sealed class MagnetPowerUp : PowerUpAction
    {
        public override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("MagnetPowerUp");
        }
    }
}