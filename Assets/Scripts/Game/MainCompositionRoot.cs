using GameCtor.DevToolbox;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniDig;
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
    //[Singleton(typeof(IPublisher<GameOverEvent>), typeof(Signal<GameOverEvent>))]
    [Singleton(typeof(Subject<GameOverEvent>))]
    [Singleton(typeof(Observable<GameOverEvent>), Factory = nameof(GetGameOverObservable))]
    [Singleton(typeof(Subject<BallCountChangedEvent>))]
    [Singleton(typeof(Observable<BallCountChangedEvent>), Factory = nameof(GetBallCountChangedObservable))]
    [Singleton(typeof(Subject<AllBricksDestroyedEvent>))]
    [Singleton(typeof(Observable<AllBricksDestroyedEvent>), Factory = nameof(GetAllBricksDestroyedObservable))]
    public partial class MainCompositionRoot : BaseCompositionRoot
    {
        [SerializeField] PowerUpTable _powerUpTable;
        [SerializeField] private Paddle _paddle;
        [SerializeField] private BrickManager _brickManager;
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private GameObject _projectilePrefab;

        private ProjectileCollisionStrategyDecorator GetProjectilePowerUp()
        {
            return new ProjectileCollisionStrategyDecorator(
                GetService<IBallPaddleCollisionStrategy>(),
                _projectilePrefab);
        }

        private PrefabFactory GetPrefabFactory()
        {
            return new PrefabFactory(this);
        }

        private Observable<GameOverEvent> GetGameOverObservable(Subject<GameOverEvent> subject) => subject;
        private Observable<BallCountChangedEvent> GetBallCountChangedObservable(Subject<BallCountChangedEvent> subject) => subject;
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
