using UnityEngine;

namespace BreakoutGame
{
    public sealed class PowerUpFactory
    {
        public PowerUp Create(PowerUpData data, Vector3 position)
        {
            var instance = Object.Instantiate(data.Config.Prefab, position, Quaternion.identity);
            instance.Init(data);
            return instance;
        }
    }
}
