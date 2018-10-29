public abstract class PowerUp
{
    public PowerUp(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    protected GameManager _gameManager;

    public abstract void ApplyEffect(Paddle paddle);
}
