using GameCtor.DevToolbox;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BreakoutGame
{
    public sealed class PowerUpFactory
    {
        private readonly Injector _injector;
        private readonly Dictionary<Type, ObjectPool<PowerUp>> _powerUpPools = new();

        public PowerUpFactory(Injector injector)
        {
            _injector = injector;
        }

        public PowerUp Create(PowerUp prefab, Vector3 position)
        {
            ULog.Trace($"Creating power-up of type {prefab.GetType().Name} at position {position}");
            if (!_powerUpPools.TryGetValue(prefab.GetType(), out var pool))
            {
                pool = new ObjectPool<PowerUp>(
                    createFunc: () =>
                    {
                        var instance = _injector.Instantiate(prefab, position, Quaternion.identity);
                        instance.Pool = pool;
                        return instance;
                    },
                    actionOnGet: powerUp =>
                    {
                        powerUp.gameObject.SetActive(true);
                    },
                    actionOnRelease: powerUp => { },
                    actionOnDestroy: powerUp =>
                    {
                        if (powerUp != null) UnityEngine.Object.Destroy(powerUp.gameObject);
                    },
                    collectionCheck: true,
                    defaultCapacity: 1,
                    maxSize: 10);
                _powerUpPools[prefab.GetType()] = pool;
            }

            var instance = pool.Get();
            instance.transform.position = position;
            return instance;
        }
    }
}
