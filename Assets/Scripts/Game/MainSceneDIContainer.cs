using GameCtor.FuseDI;
using R3;
using System;
using UnityEngine;

namespace BreakoutGame
{
    [ServiceProvider]
    [Singleton(typeof(Game))]
    [Singleton(typeof(IPowerUpSpawner), typeof(PowerUpSpawner))]
    [Singleton(typeof(IRandom), typeof(UnityRandom))]
    [Singleton(typeof(BrickManager), Instance = nameof(_brickManager))]
    //[Singleton(typeof(BrickManager), Factory = nameof(GetBrickManager))]
    [Singleton(typeof(BallManager), Instance = nameof(_ballManager))]
    [Singleton(typeof(Paddle), Instance = nameof(_paddle))]
    // [Singleton(typeof(PowerUpAction), typeof(ExtraLifePowerUpAction), Key = nameof(PowerUpKind.ExtraLife))]
    // [Singleton(typeof(PowerUpAction), typeof(ExtraBallPowerUpAction), Key = nameof(PowerUpKind.ExtraBall))]
    // [Singleton(typeof(ReverseBounceModifier), typeof(ReverseBounceModifier))]
    // [Singleton(typeof(MagnetPowerUp), typeof(MagnetPowerUp))]
    [Singleton(typeof(IBallPaddleCollisionStrategy), typeof(NormalBounceStrategy))]
    [Singleton(typeof(ReverseBounceStrategy))]
    [Singleton(typeof(MagnetBounceStrategy))]
    [Singleton(typeof(ProjectileCollisionStrategyDecorator), Factory = nameof(GetProjectilePowerUp))]
    [Singleton(typeof(HeavyBallGameWorldEffect))]
    //[Singleton(typeof(ExtraLifePowerUpAction), typeof(ExtraLifePowerUpAction))]
    [Singleton(typeof(PrefabFactory), Factory = nameof(GetPrefabFactory))]
    [Transient(typeof(PrefabFactory1), Factory = nameof(GetPrefabFactory1))]
    //[Singleton(typeof(IPublisher<GameOverEvent>), typeof(Signal<GameOverEvent>))]

    [Singleton(typeof(LifeTracker))]
    [Singleton(typeof(Subject<ExtraLifeUsedEvent>))]
    [Singleton(typeof(Observer<ExtraLifeUsedEvent>), Factory = nameof(GetExtraLifeUsedObserver))]
    [Singleton(typeof(Observable<ExtraLifeUsedEvent>), Factory = nameof(GetExtraLifeUsedObservable))]
    [Singleton(typeof(Subject<GameOverEvent>))]
    [Singleton(typeof(Observer<GameOverEvent>), Factory = nameof(GetGameOverObserver))]
    //[Singleton(typeof(Observable<GameOverEvent>), Factory = nameof(GetGameOverObservable))]
    [Singleton(typeof(Subject<BallCountChangedEvent>))]
    [Singleton(typeof(Observer<BallCountChangedEvent>), Factory = nameof(GetBallCountChangedObserver))]
    [Singleton(typeof(Observable<BallCountChangedEvent>), Factory = nameof(GetBallCountChangedObservable))]
    [Singleton(typeof(Subject<AllBricksDestroyedEvent>))]
    [Singleton(typeof(Observable<AllBricksDestroyedEvent>), Factory = nameof(GetAllBricksDestroyedObservable))]
    public partial class MainSceneDIContainer : DIContainer
    {
        [SerializeField] PowerUpTable _powerUpTable;
        [SerializeField] private Paddle _paddle;
        [SerializeField] private BrickManager _brickManager;
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private GameObject _projectilePrefab;

        //public T GetService<T>() => this is IServiceProvider<T> provider ? provider.GetService() : throw new InvalidOperationException();
        //public T GetService<T>(string diKey) => this is INamedServiceProvider<T> provider ? provider.GetService(diKey) : throw new InvalidOperationException();

        private void Start()
        {
            _paddle.TryGetComponent<Paddle>(out var paddle);
            Debug.Log($"paddle found? {paddle != null}");
            //var prefabFactory = GetService<PrefabFactory1>();
            //prefabFactory.Create(_paddle.gameObject, Vector3.zero);
            //prefabFactory.Create(_paddle.gameObject, Vector3.zero);
        }

        private ProjectileCollisionStrategyDecorator GetProjectilePowerUp(IBallPaddleCollisionStrategy collisionStrategy)
        {
            return new ProjectileCollisionStrategyDecorator(
                collisionStrategy,
                _projectilePrefab);
        }

        private PrefabFactory GetPrefabFactory()
        {
            return new PrefabFactory(this);
        }

        private PrefabFactory1 GetPrefabFactory1()
        {
            return new PrefabFactory1(this);
        }

        private Observable<ExtraLifeUsedEvent> GetExtraLifeUsedObservable(Subject<ExtraLifeUsedEvent> subject) => subject;
        private Observer<ExtraLifeUsedEvent> GetExtraLifeUsedObserver(Subject<ExtraLifeUsedEvent> subject) => subject.AsObserver();
        private Observer<GameOverEvent> GetGameOverObserver(Subject<GameOverEvent> subject) => subject.AsObserver();
        private Observable<BallCountChangedEvent> GetBallCountChangedObservable(Subject<BallCountChangedEvent> subject) => subject;
        private Observer<BallCountChangedEvent> GetBallCountChangedObserver(Subject<BallCountChangedEvent> subject) => subject.AsObserver();
        private Observable<AllBricksDestroyedEvent> GetAllBricksDestroyedObservable(Subject<AllBricksDestroyedEvent> subject) => subject;

        private PowerUpSpawner GetPowerUpSpawner()
        {
            Debug.Log(_ballManager?.ToString());
            return null;
            // var dataList = new List<PowerUpData>();
            // foreach (var config in _powerUpTable.Configs)
            // {
            //     var command = Resolve<PowerUpAction>(config.Kind.ToString());
            //     var data = new PowerUpData(config, command);
            //     dataList.Add(data);
            // }

            // return new PowerUpSpawner(
            //     dataList,
            //     new PowerUpFactory(),
            //     GetService<IRandom>());
        }
    }

    //[ServiceProviderModule]
    //[Singleton(typeof(Game))]
    //public partial class MainCompositionRoot
    //{
    //}

    public sealed class PrefabFactory
    {
        private readonly IInjector _injector;

        public PrefabFactory(IInjector injector)
        {
            _injector = injector;
        }

        public T Create<T>(T prefab, Vector3 position) where T : UnityEngine.Object
        {
            var instance = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
            _injector.Inject(instance);
            return instance;
        }
    }
}
