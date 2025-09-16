using UniRx;

namespace BreakoutGame
{
    public class Paddle
    {
        private IBallPaddleCollisionStrategy _collisionStrategy;

        public Paddle()
        {
            Width = new ReactiveProperty<float>(1);
            ResetBallPos = new ReactiveCommand<Unit>();
        }

        public IReactiveProperty<float> Width { get; }

        public ReactiveCommand<Unit> ResetBallPos { get; }

        public void SetCollisionStrategy(IBallPaddleCollisionStrategy strategy)
        {
            _collisionStrategy = strategy;
        }

        public void OnBallCollision(Ball ball)
        {
            _collisionStrategy.HandleCollision(ball, this);
        }
    }
}