using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BrickPresenter
    {
        private readonly GameObject _view;
        private readonly Brick.Config _config;
        private readonly IPowerUpSpawner _powerUpSpawner;
        private readonly IReactiveCommand<Ball> _respondToBallCollision;

        public BrickPresenter(GameObject view, Brick.Config config, IPowerUpSpawner powerUpSpawner)
        {
            // TODO: Validate input
            _view = view;
            _config = config;
            _powerUpSpawner = powerUpSpawner;

            Hp = new ReactiveProperty<int>(config._initialHp);

            Active = Hp.Select(x => x > 0).ToReactiveProperty();

            ResetHp = new ReactiveCommand<Unit>();
            ResetHp
                .Subscribe(_ => Hp.Value = config._initialHp);

            _respondToBallCollision = new ReactiveCommand<Ball>();
            _respondToBallCollision
                .Subscribe(ball => Hp.Value -= ball.Presenter.Power);

            Active
                .Where(value => !value)
                .Subscribe(_ => SpawnPowerUp());

            Active
                .Subscribe(value => view.SetActive(value));
        }

        public IReactiveProperty<int> Hp { get; }

        public IReadOnlyReactiveProperty<bool> Active { get; }

        public IReactiveCommand<Unit> ResetHp { get; }

        public void OnCollisionEnter2D(GameObject other)
        {
            if (other.TryGetComponent<Ball>(out var ball))
            {
                _respondToBallCollision.Execute(ball);
            }
        }

        private void SpawnPowerUp()
        {
            _powerUpSpawner.SpawnPowerUp(_view.transform.position);
        }
    }
}