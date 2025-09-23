using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BrickPresenter
    {
        private readonly GameObject _view;
        private readonly Brick.Config _config;

        public BrickPresenter(GameObject view, Brick.Config config)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            // TODO: Validate config values
            _config = config ?? throw new ArgumentNullException(nameof(config));

            Hp = new ReactiveProperty<int>(config._initialHp);

            Active = Hp.Select(x => x > 0).ToReactiveProperty();

            ResetHp = new ReactiveCommand<Unit>();
            ResetHp.Subscribe(_ => Hp.Value = config._initialHp);

            RespondToBallCollision = new ReactiveCommand<BallPresenter>();
            RespondToBallCollision.Subscribe(ball => Hp.Value -= ball.Power);

            PowerUpCreated = RespondToBallCollision
                .Where(_ => RandomUtil.Random.Next(0, 10) < config._powerUpSpawnOdds)
                .Select(_ => CreateRandomPowerUp());

            view
                .OnCollisionEnter2DAsObservable()
                .Select(collision => collision.collider.GetComponent<Ball>().Presenter)
                .Subscribe(ball => RespondToBallCollision.Execute(ball))
                .AddTo(view);

            this
                .PowerUpCreated
                .Subscribe(InstantiatePowerUp)
                .AddTo(view);

            this
                .Active
                .Subscribe(value => view.SetActive(value))
                .AddTo(view);
        }

        public IObservable<PowerUp> PowerUpCreated { get; }

        public IReactiveProperty<int> Hp { get; }

        public IReadOnlyReactiveProperty<bool> Active { get; }

        public IReactiveCommand<Unit> ResetHp { get; }

        public IReactiveCommand<BallPresenter> RespondToBallCollision { get; }

        private void InstantiatePowerUp(PowerUp powerUp)
        {
            var powerUpPresenter = GameObject.Instantiate(_config._powerUpPrefab, _view.transform.position, Quaternion.identity);
            //powerUpPresenter.PowerUpPresenter = powerUp;
        }

        private PowerUp CreateRandomPowerUp()
        {
            return null;
            //int randNum = RandomUtil.Random.Next(0, 3);
            //return randNum switch
            //{
            //    0 => new ExtraLifePowerUp(),
            //    1 => new ExtraBallPowerUp(),
            //    _ => new PaddleSizePowerUp(),
            //};
        }
    }
}