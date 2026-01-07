using GameCtor.DevToolbox;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Pool;

namespace BreakoutGame
{
    public sealed class ProjectilePowerUp : PowerUp
    {
        [SerializeField]
        private GameObject _prefab;

        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("");

            if (!go.transform.parent.TryGetComponent<PowerUpStateMachine>(out var fsm))
            {
                return;
            }

            var state = new ProjectileState(_prefab);
            fsm.Transition(state);
        }
    }

    public sealed class ProjectileState : IPowerUpState, ICollisionStrategy
    {
        private static ObjectPool<GameObject> _projectilePool;

        private readonly ICollisionStrategy _strategy;
        private readonly GameObject _prefab;

        public ProjectileState(GameObject prefab)
        {
            _strategy = ContactPointBounceStrategy.Instance;
            _prefab = prefab;
            _projectilePool ??= new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    var instance = GameObject.Instantiate(_prefab);
                    instance.OnDisableAsObservable().Subscribe(_ => _projectilePool.Release(instance));
                    return instance;
                },
                actionOnRelease: obj => { },
                actionOnGet: obj => obj.SetActive(true),
                actionOnDestroy: obj => { if (obj != null) GameObject.Destroy(obj); },
                collectionCheck: true,
                defaultCapacity: 2,
                maxSize: 4);
        }

        public string Name => "Projectile";

        public void Enter()
        {
            ULog.Trace("");
        }

        public void Exit()
        {
            ULog.Trace("");
        }

        public void Resolve(Collision2D collision)
        {
            _strategy.Resolve(collision);

            var position = collision.otherCollider.transform.position;
            var targetDir = -collision.GetContact(0).normal;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            var rot = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            var instance = _projectilePool.Get();
            instance.transform.SetPositionAndRotation(position, rot);
        }
    }
}
