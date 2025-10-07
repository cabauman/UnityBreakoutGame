using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BrickPresenter
    {
        private readonly GameObject _view;
        private readonly Brick.Config _config;
        //private readonly PowerUpTable _powerUpTable;
        //private readonly IPowerUpSpawner _powerUpSpawner;
        //private readonly ReactiveCommand<Ball> _respondToBallCollision;

        public BrickPresenter(
            GameObject view,
            Brick.Config config,
            PowerUpTable powerUpTable,
            IPowerUpSpawner powerUpSpawner)
        {
            // TODO: Validate input
            _view = view;
            _config = config;
            // _powerUpTable = powerUpTable;
            // _powerUpSpawner = powerUpSpawner;

            ResetHp = new ReactiveCommand<Unit>();

            Active
                .Where(value => !value)
                .Subscribe(_ => SpawnPowerUp());

            Active
                .Subscribe(value => view.SetActive(value));
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReactiveCommand<Unit> ResetHp { get; }

        public void OnCollisionEnter2D(GameObject other)
        {
            if (!other.TryGetComponent<Ball>(out var ball))
            {
                return;
            }
            foreach (var cmd in _config._hitCommands)
            {
                cmd.Execute();
            }
        }

        private void SpawnPowerUp()
        {
            //_powerUpSpawner.SpawnPowerUp(_powerUpTable, _view.transform.position);
        }
    }
}