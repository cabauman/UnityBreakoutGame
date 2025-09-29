using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class GameDraft
    {
        private readonly uint _defaultNumLives;
        private readonly List<GameObject> _bonusBalls = new();
        private readonly GamePresenter.Config _config;
        private bool _gameOver = false;

        public GameDraft(GameObject view, GamePresenter.Config config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            Ball = config._ballPresenter.Presenter;
            Paddle = config._paddlePresenter.Presenter;
            _defaultNumLives = config._defaultNumLives > 0 ? config._defaultNumLives : 1;

            NumLives = new ReactiveProperty<uint>(_defaultNumLives);
            BricksRemaining = new ReactiveProperty<int>(config._brickPresenters.Count);
            NumBallsInPlay = new ReactiveProperty<int>(1);

            GameWon = BricksRemaining
                .Where(count => count == 0)
                .Do(x => { _gameOver = true; Ball.Active.Value = false; })
                .Select(_ => Unit.Default);

            var noBallsInPlay = NumBallsInPlay
                .Where(count => count == 0)
                .Do(_ => NumLives.Value -= 1)
                .Publish()
                .RefCount();

            noBallsInPlay
                .Where(_ => NumLives.Value > 0)
                .Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UseExtraLife());

            GameLost = noBallsInPlay
                .Where(_ => NumLives.Value == 0)
                .Select(_ => Unit.Default);

            ResetGameCmd = new ReactiveCommand();
            ResetGameCmd.Subscribe(_ => ResetGame());

            CreateBonusBall = new ReactiveCommand<Vector3>(Observable.Defer(() => Observable.Return(!_gameOver)));
            CreateBonusBall
                .Do(_ => NumBallsInPlay.Value += 1)
                .Select(InstantiateBonusBall)
                .SelectMany(DetectWhenBonusBallBecomesInactive)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);

            this
                .GameWon
                .Subscribe(_ => ClearBonusBalls())
                .AddTo(view);

            //Observable
            //    .EveryUpdate()
            //    .Where(_ =>
            //        UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            //    .Subscribe(_ => _config._ballPresenter.Presenter.AddInitialForce())
            //    .AddTo(view);
        }

        public void Start()
        {
            Bricks = _config._brickPresenters.Select(x => x.Presenter).ToList();
            Ball
                .Active
                .Where(active => !_gameOver && !active)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);

            Bricks
                .ToObservable()
                .SelectMany(x =>
                {
                    return x.Active;
                })
                .Where(active => active == false)
                .Subscribe(_ => BricksRemaining.Value -= 1);
        }

        public BallPresenter Ball { get; }

        public PaddlePresenter Paddle { get; }

        public IReadOnlyList<BrickPresenter> Bricks { get; private set; }

        public IObservable<Unit> GameWon { get; }

        public IObservable<Unit> GameLost { get; }

        public IReactiveProperty<int> NumBallsInPlay { get; }

        public IReactiveProperty<int> BricksRemaining { get; }

        public IReactiveProperty<uint> NumLives { get; }

        public ReactiveCommand ResetGameCmd { get; }

        public ReactiveCommand<Vector3> CreateBonusBall { get; }

        //public void Tick(float deltaTime)
        //{
        //    if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        //    {
        //        Ball.AddInitialForce();
        //    }
        //}

        private void ResetGame()
        {
            NumLives.Value = _defaultNumLives;
            Paddle.ResetBallPos.Execute(Unit.Default);
            Ball.Active.Value = true;
            NumBallsInPlay.Value = 1;
            BricksRemaining.Value = Bricks.Count;

            foreach (var brick in Bricks)
            {
                brick.ResetHp.Execute(Unit.Default);
            }

            _gameOver = false;
        }

        private void UseExtraLife()
        {
            Paddle.ResetBallPos.Execute(Unit.Default);
            Ball.Active.Value = true;
            NumBallsInPlay.Value = 1;
        }

        private IObservable<Unit> DetectWhenBonusBallBecomesInactive(Ball ball)
        {
            return ball.Presenter.Active
                .Where(active => !active)
                .Select(_ => Unit.Default)
                .Take(1);
        }

        private Ball InstantiateBonusBall(Vector3 spawnPosition)
        {
            //IView v = null;
            //var result = v.Instantiate<BallPresenter, Ball>(_config._ballPresenterPrefab, spawnPosition, Quaternion.identity);
            var ballPresenter = GameObject.Instantiate(_config._ballPresenterPrefab, spawnPosition, Quaternion.identity);
            //ballPresenter.Ball.AddInitialForce();
            //_bonusBalls.Add(ballPresenter.gameObject);
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