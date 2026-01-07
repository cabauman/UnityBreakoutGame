using R3;

namespace BreakoutGame
{
    public interface IReadOnlyGameEvents
    {
        public ReadOnlyReactiveProperty<bool> IsPaused { get; }
        Observable<Unit> GameStarted { get; }
        Observable<Unit> GameWon { get; }
        Observable<Unit> GameLost { get; }
    }

    public sealed class GameEvents : IReadOnlyGameEvents
    {
        public ReactiveProperty<bool> IsPaused { get; } = new();
        public Subject<Unit> GameStarted { get; } = new();
        public Subject<Unit> GameWon { get; } = new();
        public Subject<Unit> GameLost { get; } = new();

        ReadOnlyReactiveProperty<bool> IReadOnlyGameEvents.IsPaused => IsPaused;
        Observable<Unit> IReadOnlyGameEvents.GameStarted => GameStarted;
        Observable<Unit> IReadOnlyGameEvents.GameWon => GameWon;
        Observable<Unit> IReadOnlyGameEvents.GameLost => GameLost;
    }
}
