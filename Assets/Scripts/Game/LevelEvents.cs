using R3;

namespace BreakoutGame
{
    public interface IReadOnlyLevelEvents
    {
        ReadOnlyReactiveProperty<bool> LevelReady { get; }
        Observable<int> LevelStarted { get; }
        Observable<int> LevelPassed { get; }
        Observable<int> LevelFailed { get; }
        //Observable<Unit> LevelPaused { get; }
    }

    public sealed class LevelEvents : IReadOnlyLevelEvents
    {
        public LevelEvents()
        {
            LevelStarted.Subscribe(_ => LevelReady.Value = true);
            Observable.Merge(LevelPassed, LevelFailed)
                .Subscribe(_ => LevelReady.Value = false);
        }

        public ReactiveProperty<bool> LevelReady { get; } = new();
        public Subject<int> LevelStarted { get; } = new();
        public Subject<int> LevelPassed { get; } = new();
        public Subject<int> LevelFailed { get; } = new();
        //public Subject<Unit> LevelPaused { get; } = new();

        ReadOnlyReactiveProperty<bool> IReadOnlyLevelEvents.LevelReady => LevelReady;
        Observable<int> IReadOnlyLevelEvents.LevelStarted => LevelStarted;
        Observable<int> IReadOnlyLevelEvents.LevelPassed => LevelPassed;
        Observable<int> IReadOnlyLevelEvents.LevelFailed => LevelFailed;
        //Observable<Unit> IReadOnlyLevelEvents.LevelPaused => LevelPaused;
    }
}
