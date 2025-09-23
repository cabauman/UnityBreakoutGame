using System;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddleSizePowerUp : PowerUpAction
    {
        [SerializeField]
        private float _widthMultiplier = 1.5f;

        [SerializeField]
        private float _effectDuration = 5f;

        public override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("Paddle size increased!");
            paddle.Presenter.Width.Value *= _widthMultiplier;
            Observable
                .Timer(TimeSpan.FromSeconds(_effectDuration))
                .Subscribe(_ => paddle.Presenter.Width.Value /= _widthMultiplier);
        }
    }
}