using System.Linq;
using UnityEngine;

namespace BreakoutGame
{
    public interface IPowerUpSpawner
    {
        void SpawnPowerUp(PowerUpTable spawnTable, Vector3 position);
    }

    public sealed class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private PowerUpTable _dropTable;

        private PowerUpFactory _factory;
        private IRandom _random;

        private void Awake()
        {
            _factory = new PowerUpFactory();
            _random = new UnityRandom();
        }

        //public PowerUpSpawner(PowerUpFactory factory, IRandom random)
        //{
        //    _factory = factory;
        //    _random = random;
        //}

        public void SpawnPowerUp()
        {
            Debug.Log("PowerUpSpawner: SpawnPowerUp called.");
            return;

            float chance = _random.Next(0f, 1f);
            if (chance > _dropTable.DropChance)
            {
                return;
            }

            PowerUpPresenter prefab = GetRandomPowerUpConfig();
            if (prefab != null)
            {
                _factory.Create(prefab, transform.position);
            }
        }

        private PowerUpPresenter GetRandomPowerUpConfig()
        {
            int totalWeight = _dropTable.Configs.Sum(item => item.Weight);
            int randomValue = _random.Next(0, totalWeight);
            int cumulative = 0;

            foreach (var item in _dropTable.Configs)
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