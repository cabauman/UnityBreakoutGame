using System;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddleSizePowerUp : PowerUpPresenter
    {
        private readonly float _widthMultiplier = 1.5f;
        private readonly float _effectDuration = 5f;

        public override void ApplyEffect(Game game, Vector3 position)
        {
            Debug.Log("Paddle size increased!");
            game.Paddle.Width.Value *= _widthMultiplier;
            Observable
                .Timer(TimeSpan.FromSeconds(_effectDuration))
                .Subscribe(_ => game.Paddle.Width.Value /= _widthMultiplier);
        }
    }
}
