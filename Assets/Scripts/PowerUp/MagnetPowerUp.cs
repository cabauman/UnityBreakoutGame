using UnityEngine;

namespace BreakoutGame
{
    public sealed class MagnetPowerUp : PowerUpAction
    {
        private readonly MagnetBounceStrategy _strategy;

        public MagnetPowerUp(MagnetBounceStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("MagnetPowerUp");
            paddle.Presenter.SetCollisionStrategy(_strategy);
        }
    }
}