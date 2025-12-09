using R3;

namespace BreakoutGame
{
    public sealed class Paddle
    {
        public Paddle()
        {
            Width = new ReactiveProperty<float>(1);
            ResetBallPos = new ReactiveCommand<Unit>();
        }

        public ReactiveProperty<float> Width { get; }

        public ReactiveCommand<Unit> ResetBallPos { get; }
    }
}
