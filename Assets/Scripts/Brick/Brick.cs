using System;
using UniRx;

namespace BreakoutGame
{
    public class Brick
    {
        private readonly int _initialHp = 1;
        private readonly int _powerUpSpawnOdds = 3;

        public Brick(int initialHp, int powerUpSpawnOdds)
        {
            _initialHp = initialHp > 0 ? initialHp : 1;
            _powerUpSpawnOdds = powerUpSpawnOdds;

            Hp = new ReactiveProperty<int>(_initialHp);

            Active = Hp.Select(x => x > 0).ToReactiveProperty();

            ResetHp = new ReactiveCommand<Unit>();
            ResetHp.Subscribe(_ => Hp.Value = _initialHp);

            RespondToBallCollision = new ReactiveCommand<Ball>();
            RespondToBallCollision.Subscribe(ball => Hp.Value -= ball.Power);

            PowerUpCreated = RespondToBallCollision
                .Where(_ => RandomUtil.Random.Next(0, 10) < _powerUpSpawnOdds)
                .Select(_ => CreateRandomPowerUp());
        }

        public IObservable<PowerUp> PowerUpCreated { get; }

        public IReactiveProperty<int> Hp { get; }

        public IReadOnlyReactiveProperty<bool> Active { get; }

        public IReactiveCommand<Unit> ResetHp { get; }

        public IReactiveCommand<Ball> RespondToBallCollision { get; }

        private PowerUp CreateRandomPowerUp()
        {
            int randNum = RandomUtil.Random.Next(0, 3);
            return randNum switch
            {
                0 => new ExtraLifePowerUp(),
                1 => new ExtraBallPowerUp(),
                _ => new PaddleSizePowerUp(),
            };
        }
    }
}