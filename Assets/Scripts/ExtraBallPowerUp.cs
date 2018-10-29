using System;
using UniRx;

public class ExtraBallPowerUp : PowerUp
{
    public ExtraBallPowerUp(GameManager gameManager)
        : base(gameManager)
    {
    }

    public override void ApplyEffect(Paddle paddle)
    {
        UnityEngine.Debug.Log("Extra ball!");
    }
}
