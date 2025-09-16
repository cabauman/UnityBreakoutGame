using UniRx;

namespace BreakoutGame
{
    public interface IBall
    {
        float InitialForce { get; }

        int Power { get; }

        IReactiveProperty<bool> Active { get; }
    }
}