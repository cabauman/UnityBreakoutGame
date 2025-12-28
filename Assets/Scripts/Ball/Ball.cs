using GameCtor.DevToolbox;
using UnityEngine;
using UnityEngine.Pool;

namespace BreakoutGame
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public sealed class Ball : MonoBehaviour
    {
        [SerializeField]
        private Power _power;

        private ObjectPool<Ball> _owningPool;

        public int Power
        {
            get => _power.Value;
            set => _power.Value = value;
        }

        public void SetOwningPool(ObjectPool<Ball> pool)
        {
            _owningPool = pool;
        }

        public void ReleaseToPool()
        {
            if (_owningPool != null)
            {
                _owningPool.Release(this);
            }
            else
            {
                ULog.Warn("Owning pool is not set for this ball.");
            }
        }

        private void Awake()
        {
            Ensure.NotNull(_power);
        }
    }
}
