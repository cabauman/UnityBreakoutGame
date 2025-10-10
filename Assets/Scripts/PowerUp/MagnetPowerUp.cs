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

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("MagnetPowerUp");
            paddle.Presenter.SetGameWorldEffect(_strategy);
        }
    }
}