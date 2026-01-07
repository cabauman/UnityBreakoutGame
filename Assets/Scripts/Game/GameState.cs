using R3;

namespace BreakoutGame
{
    public interface IReadOnlyGameState
    {
        ReadOnlyReactiveProperty<int> NumLives { get; }
        ReadOnlyReactiveProperty<int> Level { get; }
    }

    public interface IReadOnlyPlayerEvents
    {
        Observable<Unit> LifeLost { get; }
        Observable<Unit> PlayerSpawned { get; }
    }
}
