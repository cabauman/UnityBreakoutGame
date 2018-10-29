using System;
using UniRx;

public class ExtraLifePowerUp : PowerUp
{
    public ExtraLifePowerUp(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void ApplyEffect(Paddle paddle)
    {
        UnityEngine.Debug.Log("Extra life!");
        _gameManager.NumLives.Value += 1;
    }
}
