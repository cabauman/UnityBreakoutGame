using System;
using GameCtor.DevToolbox;
using R3;

namespace BreakoutGame
{
    public sealed class Brick
    {
        private readonly int _initialHp = 1;
        private readonly int _powerUpSpawnOdds = 3;

        public Brick(int initialHp, int powerUpSpawnOdds)
        {
            _initialHp = initialHp > 0 ? initialHp : 1;
            _powerUpSpawnOdds = powerUpSpawnOdds;

            Hp = new ReactiveProperty<int>(_initialHp);

            Active = Hp.Select(x => x > 0).ToReadOnlyReactiveProperty();

            ResetHp = new ReactiveCommand<Unit>();
            ResetHp.Subscribe(_ => Hp.Value = _initialHp);

            RespondToBallCollision = new ReactiveCommand<Ball>();
            RespondToBallCollision.Subscribe(ball => Hp.Value -= ball.Power);

            CreatePowerUp = RespondToBallCollision
                .Where(_ => RandomUtil.Random.Next(0, 10) < _powerUpSpawnOdds)
                .AsUnitObservable();
        }

        public Observable<Unit> CreatePowerUp { get; }

        public ReactiveProperty<int> Hp { get; }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReactiveCommand<Unit> ResetHp { get; }

        public ReactiveCommand<Ball> RespondToBallCollision { get; }
    }
}
