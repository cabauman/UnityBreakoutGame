using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using System.Linq;
using UnityEngine;

namespace BreakoutGame
{
    public interface IPowerUpSpawner
    {
        void SpawnPowerUp(PowerUpTable spawnTable, Vector3 position);
    }

    public sealed partial class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField]
        private PowerUpTable _dropTable;

        [Inject]
        private PowerUpFactory _factory;

        [Inject]
        private IRandom _random;

        public void SpawnPowerUp()
        {
            ULog.Trace("");
            float chance = _random.Next(0f, 1f);
            if (chance > _dropTable.DropChance)
            {
                return;
            }

            PowerUp prefab = GetRandomPowerUpConfig();
            if (prefab != null)
            {
                _factory.Create(prefab, transform.position);
            }
        }

        private void Awake()
        {
            Ensure.NotNull(_dropTable);
        }

        private PowerUp GetRandomPowerUpConfig()
        {
            int totalWeight = _dropTable.Configs.Sum(item => item.Weight);
            int randomValue = _random.Next(0, totalWeight);
            int cumulative = 0;

            ULog.Trace($"totalWeight={totalWeight}, randomValue={randomValue}");

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