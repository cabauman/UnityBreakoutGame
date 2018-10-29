using System;
using UniRx;

public class Brick
{
    private int _initalHp = 1;

    public Brick(int initialHp)
    {
        _initalHp = initialHp;

        Hp = new ReactiveProperty<int>(_initalHp);

        Active = Hp.Select(x => x > 0).ToReactiveProperty();

        ResetHp = new ReactiveCommand<Unit>();
        ResetHp.Subscribe(_ => Hp.Value = _initalHp);

        RespondToBallCollision = new ReactiveCommand<Ball>();
        RespondToBallCollision.Subscribe(ball => Hp.Value -= ball.Power);

        PowerUpCreated = RespondToBallCollision
            .Where(_ => RandomUtil.Random.Next(0, 10) <= 4)
            .Select(_ => CreateRandomPowerUp());
    }

    public GameManager GameManager { get; set; }

    public IObservable<PowerUp> PowerUpCreated { get; }

    public IReactiveProperty<int> Hp { get; }

    public IReadOnlyReactiveProperty<bool> Active { get; }

    public IReactiveCommand<Unit> ResetHp { get; }

    public IReactiveCommand<Ball> RespondToBallCollision { get; }

    private PowerUp CreateRandomPowerUp()
    {
        int randNum = RandomUtil.Random.Next(0, 2);
        switch (randNum)
        {
            case 0:
                return new ExtraLifePowerUp(GameManager);
            default:
                return new PaddleSizePowerUp(GameManager);
        }
    }
}
