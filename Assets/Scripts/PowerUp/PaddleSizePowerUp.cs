using System;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class PaddleSizePowerUp : PowerUpPresenter
    {
        [SerializeField]
        private float _widthMultiplier = 1.5f;

        [SerializeField]
        private float _effectDuration = 5f;

        protected override void ApplyEffect(PaddlePresenter paddle)
        {
            Debug.Log("Paddle size increased!");
            paddle.Paddle.Width.Value *= _widthMultiplier;
            Observable
                .Timer(TimeSpan.FromSeconds(_effectDuration))
                .Subscribe(_ => paddle.Paddle.Width.Value /= _widthMultiplier);
        }
    }
}