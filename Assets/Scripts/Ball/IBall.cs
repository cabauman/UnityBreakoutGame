using R3;

namespace BreakoutGame
{
    public interface IBall
    {
        float InitialForce { get; }

        int Power { get; }

        ReactiveProperty<bool> Active { get; }
    }
}
