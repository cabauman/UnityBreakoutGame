using GameCtor.DevToolbox;
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
    [Singleton(typeof(PowerUpFactory), typeof(PowerUpFactory))]
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
    [Singleton(typeof(ReverseBounceStrategy), typeof(ReverseBounceStrategy))]
    [Singleton(typeof(ReverseBounceStrategy), typeof(ReverseBounceStrategy), Key = "A")]
    [Singleton(typeof(MagnetBounceStrategy), typeof(MagnetBounceStrategy))]
    public partial class MainCompositionRoot : BaseCompositionRoot
    {
        [SerializeField] PowerUpTable _powerUpTable;
        [SerializeField] private Paddle _paddle;
        [SerializeField] private BrickManager _brickManager;
        [SerializeField] private BallManager _ballManager;

        private BrickManager GetBrickManager()
        {
            _brickManager.Inject(GetService<IRandom>());
            return _brickManager;
        }

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
}
