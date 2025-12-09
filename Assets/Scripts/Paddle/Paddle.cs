using UniRx;

namespace BreakoutGame
{
    public sealed class Paddle
    {
        public Paddle()
        {
            Width = new ReactiveProperty<float>(1);
            ResetBallPos = new ReactiveCommand<Unit>();
        }

        public IReactiveProperty<float> Width { get; }

        public ReactiveCommand<Unit> ResetBallPos { get; }
    }
}
