public class ExtraLifePowerUp : PowerUp
{
    public override void ApplyEffect(Game game, Paddle paddle)
    {
        UnityEngine.Debug.Log("Extra life!");
        game.NumLives.Value += 1;
    }
}
