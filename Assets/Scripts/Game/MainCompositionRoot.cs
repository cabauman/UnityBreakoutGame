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
    [Singleton(typeof(IPowerUpSpawner), Factory = nameof(GetPowerUpSpawner))]
    [Scoped(typeof(IRandom), typeof(UnityRandom))]
    [Singleton(typeof(BrickManager), Instance = nameof(_brickManager))]
    [Singleton(typeof(BallManager), Instance = nameof(_ballManager))]
    [Singleton(typeof(Paddle), Instance = nameof(_paddle))]
    [Singleton(typeof(PowerUpAction), typeof(ExtraLifePowerUpAction), Key = nameof(PowerUpKind.ExtraLife))]
    [Singleton(typeof(PowerUpAction), typeof(ExtraBallPowerUpAction), Key = nameof(PowerUpKind.ExtraBall))]
    public partial class MainCompositionRoot : BaseCompositionRoot
    {
        [SerializeField] PowerUpTable _powerUpTable;
        [SerializeField] private Paddle _paddle;
        [SerializeField] private BrickManager _brickManager;
        [SerializeField] private BallManager _ballManager;

        private PowerUpSpawner GetPowerUpSpawner()
        {
            var dataList = new List<PowerUpData>();
            foreach (var config in _powerUpTable.Configs)
            {
                var command = Resolve<PowerUpAction>(config.Kind.ToString());
                var data = new PowerUpData(config, command);
                dataList.Add(data);
            }

            return new PowerUpSpawner(
                dataList,
                new PowerUpFactory(),
                GetService<IRandom>());
        }

        public override void Dispose1() => Dispose();
    }
}
