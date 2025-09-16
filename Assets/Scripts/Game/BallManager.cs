using System;
using System.Linq;
using UniRx;

namespace BreakoutGame
{
    public sealed class BallManager
    {
        private bool _gameOver = false;

        public BallManager()
        {
            //Ball = ball;
            NumBallsInPlay = new ReactiveProperty<int>(1);

            Ball.Active
                .Where(active => !active)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);

            CreateBonusBall = new ReactiveCommand<Ball>(Observable.Defer(() => Observable.Return(!_gameOver)));
            CreateBonusBall
                .Do(_ => NumBallsInPlay.Value += 1)
                .SelectMany(DetectWhenBonusBallBecomesInactive)
                .Subscribe(_ => NumBallsInPlay.Value -= 1);
        }

        public Ball Ball { get; }

        public IReactiveProperty<int> NumBallsInPlay { get; }

        public ReactiveCommand<Ball> CreateBonusBall { get; }

        private IObservable<Unit> DetectWhenBonusBallBecomesInactive(Ball ball)
        {
            return ball.Active
                .Where(active => !active)
                .Select(_ => Unit.Default)
                .Take(1);
        }
    }
}