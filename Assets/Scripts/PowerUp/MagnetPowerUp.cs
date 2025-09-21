using UnityEngine;

namespace BreakoutGame
{
    public class MagnetPowerUp : PowerUp
    {
        public override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("MagnetPowerUp");
        }
    }
}