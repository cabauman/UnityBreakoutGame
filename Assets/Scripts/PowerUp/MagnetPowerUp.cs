using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class MagnetPowerUp : PowerUp
    {
        [Inject] private MagnetBounceStrategy _strategy;

        private void Start()
        {
            Debug.Log(_strategy == null ? "null" : "not null");
        }
        
        // private readonly MagnetBounceStrategy _strategy;

        // public MagnetPowerUp(MagnetBounceStrategy strategy)
        // {
        //     _strategy = strategy;
        // }

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("MagnetPowerUp");
            paddle.Presenter.SetCollisionStrategy(_strategy);
        }
    }
}