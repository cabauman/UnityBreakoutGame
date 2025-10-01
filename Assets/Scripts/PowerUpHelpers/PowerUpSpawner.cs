using System.Linq;
using UnityEngine;

namespace BreakoutGame
{
    public interface IPowerUpSpawner
    {
        void SpawnPowerUp(PowerUpTable spawnTable, Vector3 position);
    }
    
    public sealed class PowerUpSpawner : IPowerUpSpawner
    {
        private readonly PowerUpFactory _factory;
        private readonly IRandom _random;

        public PowerUpSpawner(PowerUpFactory factory, IRandom random)
        {
            _factory = factory;
            _random = random;
        }

        public void SpawnPowerUp(PowerUpTable spawnTable, Vector3 position)
        {
            PowerUp prefab = GetRandomPowerUpConfig(spawnTable);
            if (prefab != null)
            {
                _factory.Create(prefab, position);
            }
        }

        private PowerUp GetRandomPowerUpConfig(PowerUpTable spawnTable)
        {
            int totalWeight = spawnTable.Configs.Sum(item => item.Weight);
            int randomValue = _random.Next(0, totalWeight);
            int cumulative = 0;

            foreach (var item in spawnTable.Configs)
            {
                cumulative += item.Weight;
                if (randomValue < cumulative)
                {
                    return item.Prefab;
                }
            }

            return null;
        }
    }
}