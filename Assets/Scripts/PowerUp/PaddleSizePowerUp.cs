using System;
using UniRx;
using UnityEngine;

public class PaddleSizePowerUp : PowerUp
{
    private float _widthMultiplier = 1.5f;
    private float _effectDuration = 5f;

    public override void ApplyEffect(Game game, Vector3 position)
    {
        UnityEngine.Debug.Log("Paddle size increased!");
        game.Paddle.Width.Value *= _widthMultiplier;
        Observable
            .Timer(TimeSpan.FromSeconds(_effectDuration))
            .Subscribe(_ => game.Paddle.Width.Value /= _widthMultiplier);
    }
}
