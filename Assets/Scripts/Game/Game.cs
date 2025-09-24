using System;
using System.Linq;
using UniRx;

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
            _brickManager = brickManager;
            _ballManager = ballManager;
            _paddle = paddle;
            _defaultNumLives = 1;

            NumLives = new ReactiveProperty<uint>(_defaultNumLives);

            var noBallsInPlay = ballManager.NumBallsInPlay
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

            GameWon = brickManager.BricksRemaining
                .Where(count => count == 0)
                .Do(_ => _gameOver = true)
                .Select(static _ => Unit.Default);

            ResetGameCmd = new ReactiveCommand();
            ResetGameCmd.Subscribe(_ => ResetGame());
        }

        public IObservable<Unit> GameWon { get; }

        public IObservable<Unit> GameLost { get; }

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