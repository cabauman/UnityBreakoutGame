using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace BreakoutGame
{
    public sealed class Game
    {
        private readonly uint _defaultNumLives;
        private bool _gameOver = false;

        public Game(Ball ball, Paddle paddle, IReadOnlyList<Brick> bricks, uint defaultNumLives)
        {
            Ball = ball;
            Paddle = paddle;
            Bricks = bricks;
            _defaultNumLives = defaultNumLives > 0 ? defaultNumLives : 1;
            NumLives = new ReactiveProperty<uint>(_defaultNumLives);
            BricksRemaining = new ReactiveProperty<int>(Bricks.Count);
            NumBallsInPlay = new ReactiveProperty<int>(1);

            Ball.Active
                .Where(active => !_gameOver && !active)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);

            Bricks
                .ToObservable()
                .SelectMany(x => x.Active)
                .Where(active => active == false)
                .Subscribe(_ => BricksRemaining.Value -= 1);

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

            CreateBonusBall = new ReactiveCommand<Ball>(Observable.Defer(() => Observable.Return(!_gameOver)));
            CreateBonusBall
                .Do(_ => NumBallsInPlay.Value += 1)
                .SelectMany(bonusBall => DetectWhenBonusBallBecomesInactive(bonusBall))
                .Subscribe(_ => NumBallsInPlay.Value -= 1);
        }

        public Ball Ball { get; }

        public Paddle Paddle { get; }

        public IReadOnlyList<Brick> Bricks { get; }

        public IObservable<Unit> GameWon { get; }

        public IObservable<Unit> GameLost { get; }

        public IReactiveProperty<int> NumBallsInPlay { get; }

        public IReactiveProperty<int> BricksRemaining { get; }

        public IReactiveProperty<uint> NumLives { get; }

        public ReactiveCommand ResetGameCmd { get; }

        public ReactiveCommand<Ball> CreateBonusBall { get; }

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
            return ball.Active
                .Where(active => !active)
                .Select(_ => Unit.Default)
                .Take(1);
        }
    }
}
