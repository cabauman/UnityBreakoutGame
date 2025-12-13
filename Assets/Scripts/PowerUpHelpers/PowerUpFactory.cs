using UnityEngine;
using UnityEngine.Pool;

namespace BreakoutGame
{
    public sealed class PowerUpFactory
    {
        //private readonly IObjectPool<PowerUp> _pool;

        //public PowerUpFactory(IObjectPool<PowerUp> pool)
        //{
        //    _pool = pool;
        //}

        public PowerUpPresenter Create(PowerUpPresenter prefab, Vector3 position)
        {
            var instance = Object.Instantiate(prefab, position, Quaternion.identity);
            //instance.Init();
            return instance;
        }
    }
}
