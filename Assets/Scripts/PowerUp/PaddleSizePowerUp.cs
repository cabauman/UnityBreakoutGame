using System;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class PaddleSizePowerUp : PowerUp
    {
        private readonly float _widthMultiplier = 1.5f;
        private readonly float _effectDuration = 5f;

        public override string SpriteName { get; } = "PaddleWidthPowerUp";

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