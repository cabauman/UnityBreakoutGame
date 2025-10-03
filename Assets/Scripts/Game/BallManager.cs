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

        private readonly List<GameObject> _bonusBalls = new();

        private void Awake()
        {
            NumBallsInPlay = new ReactiveProperty<int>(1);

            Ball.Presenter
                .Active
                .Where(static active => !active)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);

            CreateBonusBall = new ReactiveCommand<Vector3>(); //Observable.Defer(() => Observable.Return(!_gameOver))
            CreateBonusBall
                .Do(_ => NumBallsInPlay.Value += 1)
                .Select(InstantiateBonusBall)
                .SelectMany(DetectWhenBonusBallBecomesInactive)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);
        }

        public Ball Ball => _mainBall;

        public IReactiveProperty<int> NumBallsInPlay { get; private set; }

        public ReactiveCommand<Vector3> CreateBonusBall { get; private set; }

        private static IObservable<Unit> DetectWhenBonusBallBecomesInactive(Ball ball)
        {
            return ball.Presenter
                .Active
                .Where(static active => !active)
                .Select(static _ => Unit.Default)
                .Take(1);
        }

        private Ball InstantiateBonusBall(Vector3 spawnPosition)
        {
            var ballPresenter = GameObject.Instantiate(_ballPrefab, spawnPosition, Quaternion.identity);
            ballPresenter.Presenter.SetForce(Vector2.one);
            _bonusBalls.Add(ballPresenter.gameObject);
            return ballPresenter;
        }

        public void UseExtraLife()
        {
            NumBallsInPlay.Value = 1;
        }

        public void ResetGame()
        {
            NumBallsInPlay.Value = 1;

            foreach (var ball in _bonusBalls)
            {
                GameObject.Destroy(ball);
            }

            _bonusBalls.Clear();
        }
    }
}