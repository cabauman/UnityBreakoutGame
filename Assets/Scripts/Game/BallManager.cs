using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BallManager : MonoBehaviour
    {
        [SerializeField]
        private Ball _ballPrefab;
        [SerializeField]
        private Ball _mainBall;

        private bool _gameOver = false;
        private readonly List<GameObject> _bonusBalls = new();

        private void Awake()
        {
            NumBallsInPlay = new ReactiveProperty<int>(1);

            Ball.Active
                .Where(active => !active)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);

            CreateBonusBall = new ReactiveCommand<Vector3>(Observable.Defer(() => Observable.Return(!_gameOver)));
            CreateBonusBall
                .Do(_ => NumBallsInPlay.Value += 1)
                .Select(InstantiateBonusBall)
                .SelectMany(DetectWhenBonusBallBecomesInactive)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);
        }

        public BallPresenter Ball => _mainBall.Presenter;

        public IReactiveProperty<int> NumBallsInPlay { get; private set; }

        public ReactiveCommand<Vector3> CreateBonusBall { get; private set; }

        private IObservable<Unit> DetectWhenBonusBallBecomesInactive(Ball ball)
        {
            return ball.Presenter.Active
                .Where(active => !active)
                .Select(_ => Unit.Default)
                .Take(1);
        }

        private Ball InstantiateBonusBall(Vector3 spawnPosition)
        {
            var ballPresenter = GameObject.Instantiate(_ballPrefab, spawnPosition, Quaternion.identity);
            ballPresenter.Presenter.AddInitialForce();
            _bonusBalls.Add(ballPresenter.gameObject);
            return ballPresenter;
        }

        private void ClearBonusBalls()
        {
            foreach (var ball in _bonusBalls)
            {
                GameObject.Destroy(ball);
            }

            _bonusBalls.Clear();
        }
    }
}