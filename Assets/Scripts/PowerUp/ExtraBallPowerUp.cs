using UnityEngine;

public class ExtraBallPowerUp : PowerUp
{
    public override void ApplyEffect(Game game, Vector3 position)
    {
        UnityEngine.Debug.Log("Extra ball!");
        game.CreateBonusBall.Execute(new Ball(50, 1, position));
    }
}
