using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BreakoutGame
{
    public interface IPowerUpSpawner
    {
        void SpawnPowerUp(Vector3 position);
    }
    
    public sealed class PowerUpSpawner : IPowerUpSpawner
    {
        private readonly IReadOnlyList<PowerUpData> _dataList;
        private readonly PowerUpFactory _factory;
        private readonly IRandom _random;

        public PowerUpSpawner(IReadOnlyList<PowerUpData> dataList, PowerUpFactory factory, IRandom random)
        {
            _dataList = dataList;
            _factory = factory;
            _random = random;
        }

        public void SpawnPowerUp(Vector3 position)
        {
            PowerUpData data = GetRandomPowerUpConfig();
            if (data != null)
            {
                _factory.Create(data, position);
            }
        }

        private PowerUpData GetRandomPowerUpConfig()
        {
            int totalWeight = _dataList.Sum(item => item.Config.Weight);
            int randomValue = _random.Next(0, totalWeight);
            int cumulative = 0;

            foreach (var item in _dataList)
            {
                cumulative += item.Config.Weight;
                if (randomValue < cumulative)
                {
                    return item.Config.Kind == PowerUpKind.None ? null : item;
                }
            }

            return null;
        }
    }
}