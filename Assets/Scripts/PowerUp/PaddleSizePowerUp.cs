using System;
using UniRx;

public class PaddleSizePowerUp : PowerUp
{
    private float _widthMultiplier = 1.5f;
    private float _effectDuration = 5f;

    public override void ApplyEffect(Game game, Paddle paddle)
    {
        UnityEngine.Debug.Log("Paddle size increased!");
        paddle.Width.Value *= _widthMultiplier;
        Observable
            .Timer(TimeSpan.FromSeconds(_effectDuration))
            .Subscribe(_ => paddle.Width.Value /= _widthMultiplier);
    }
}
