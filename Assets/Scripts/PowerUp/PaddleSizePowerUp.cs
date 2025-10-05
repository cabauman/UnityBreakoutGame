using System;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddleSizePowerUp : PowerUp
    {
        [SerializeField]
        private float _widthMultiplier = 1.5f;

        [SerializeField]
        private float _effectDuration = 5f;

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("Paddle size increased!");
            paddle.Presenter.WidthScale.Value *= _widthMultiplier;
            Observable
                .Timer(TimeSpan.FromSeconds(_effectDuration))
                .Subscribe(_ => paddle.Presenter.WidthScale.Value /= _widthMultiplier);
        }
    }
}