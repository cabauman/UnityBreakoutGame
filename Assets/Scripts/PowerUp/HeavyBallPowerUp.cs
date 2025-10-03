using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class HeavyBallPowerUp : PowerUp
    {
        [Inject] private BrickManager _brickManager;

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("HeavyBallPowerUp");
            _brickManager.MarkBricksAsTriggers(true);
        }
    }
}