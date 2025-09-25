using GameCtor.DevToolbox;
using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Game
    {
        private readonly BrickManager _brickManager;
        private readonly BallManager _ballManager;
        private readonly Paddle _paddle;
        private readonly uint _defaultNumLives;
        private bool _gameOver = false;

        public Game(BrickManager brickManager, BallManager ballManager, Paddle paddle)
        {
            Debug.Log("Game Constructor");
            _brickManager = brickManager;
            _ballManager = ballManager;
            _paddle = paddle;
            _defaultNumLives = 1;

            NumLives = new ReactiveProperty<uint>(_defaultNumLives);

            ResetGameCmd = new ReactiveCommand();
            ResetGameCmd.Subscribe(_ => ResetGame());
            //GameCtor.DevToolbox.StartupLifecycle.AddPostInjectListener(PostInject);

            var noBallsInPlay = _ballManager.NumBallsInPlay
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
                .Select(static _ => Unit.Default);

            GameWon = _brickManager.BricksRemaining
                .Where(count => count == 0)
                .Do(_ => _gameOver = true)
                .Select(static _ => Unit.Default);

            Debug.Log(_brickManager.Random);
        }

        public void PostInject()
        {
            Debug.Log("Game PostInject");
            //Debug.Break();

            var noBallsInPlay = _ballManager.NumBallsInPlay
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
                .Select(static _ => Unit.Default);

            GameWon = _brickManager.BricksRemaining
                .Where(count => count == 0)
                .Do(_ => _gameOver = true)
                .Select(static _ => Unit.Default);
        }

        public IObservable<Unit> GameWon { get; private set; }

        public IObservable<Unit> GameLost { get; private set; }

        public IReactiveProperty<uint> NumLives { get; }

        public ReactiveCommand ResetGameCmd { get; }

        private void ResetGame()
        {
            NumLives.Value = _defaultNumLives;
            _paddle.Presenter.ResetBallPos.Execute(Unit.Default);
            _brickManager.ResetGame();
            _ballManager.ResetGame();
            _gameOver = false;
        }

        private void UseExtraLife()
        {
            _paddle.Presenter.ResetBallPos.Execute(Unit.Default);
        }
    }
}