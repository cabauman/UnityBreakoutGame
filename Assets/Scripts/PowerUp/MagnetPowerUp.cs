using UnityEngine;

namespace BreakoutGame
{
    public class MagnetPowerUp : PowerUpPresenter
    {
        protected override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("MagnetPowerUp");
        }
    }
}