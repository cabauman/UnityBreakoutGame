using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using UnityEngine;

namespace BreakoutGame
{
    [ServiceProvider]
    [Singleton(typeof(GameEvents))]
    [Singleton(typeof(LevelEvents))]
    [Singleton(typeof(IReadOnlyGameEvents), Factory = nameof(GetGameEvents))]
    [Singleton(typeof(IReadOnlyLevelEvents), Factory = nameof(GetLevelEvents))]
    [Singleton(typeof(Injector))]
    [Singleton(typeof(IServiceResolver), Instance = "GetIServiceResolver")]
    [Singleton(typeof(PowerUpFactory))]
    [Singleton(typeof(IRandom), typeof(UnityRandom))]
    [Singleton(typeof(BallManager), Instance = nameof(_ballManager))]
    [Singleton(typeof(LifeTracker), Instance = nameof(_lifeTracker))]
    public partial class MainSceneDIContainer : DIContainer, IServiceResolver
    {
        [SerializeField]
        private BallManager _ballManager;

        [SerializeField]
        private LifeTracker _lifeTracker;

        private IReadOnlyGameEvents GetGameEvents(GameEvents gameEvents) => gameEvents;
        private IReadOnlyLevelEvents GetLevelEvents(LevelEvents levelEvents) => levelEvents;
        private IServiceResolver GetIServiceResolver => this;
    }
}
