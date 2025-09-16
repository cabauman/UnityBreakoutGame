using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace BreakoutGame
{
    public sealed class Game2
    {
        private readonly uint _defaultNumLives;
        private bool _gameOver = false;

        public Game2(Ball ball, Paddle paddle, IReadOnlyList<Brick> bricks, uint defaultNumLives)
        {
            var ballManager = new BallManager();
            Paddle = paddle;
            Bricks = bricks;
            _defaultNumLives = defaultNumLives > 0 ? defaultNumLives : 1;
            NumLives = new ReactiveProperty<uint>(_defaultNumLives);
            BricksRemaining = new ReactiveProperty<int>(Bricks.Count);

            Bricks
                .ToObservable()
                .SelectMany(x => x.Active)
                .Where(active => active == false)
                .Subscribe(_ => BricksRemaining.Value -= 1);

            GameWon = BricksRemaining
                .Where(count => count == 0)
                .Do(x => { _gameOver = true; /*Ball.Active.Value = false;*/ })
                .Select(_ => Unit.Default);

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
                .Select(_ => Unit.Default);

            ResetGameCmd = new ReactiveCommand();
            ResetGameCmd.Subscribe(_ => ResetGame());
        }

        public Paddle Paddle { get; }

        public IReadOnlyList<Brick> Bricks { get; }

        public IObservable<Unit> GameWon { get; }

        public IObservable<Unit> GameLost { get; }

        public IReactiveProperty<int> BricksRemaining { get; }

        public IReactiveProperty<uint> NumLives { get; }

        public ReactiveCommand ResetGameCmd { get; }

        private void ResetGame()
        {
            NumLives.Value = _defaultNumLives;
            Paddle.ResetBallPos.Execute(Unit.Default);
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
        }
    }
}