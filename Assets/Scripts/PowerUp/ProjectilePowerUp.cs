using UnityEngine;

namespace BreakoutGame
{
    public sealed class ProjectilePowerUp : PowerUpPresenter
    {
        [SerializeField]
        private GameObject _prefab;

        public override void ApplyEffect(GameObject go)
        {
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
        private readonly ICollisionStrategy _strategy;
        private readonly GameObject _prefab;

        public ProjectileState(GameObject prefab)
        {
            _strategy = ContactPointBounceStrategy.Instance;
            _prefab = prefab;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Resolve(Collision2D collision)
        {
            _strategy.Resolve(collision);

            var position = collision.otherCollider.transform.position;
            var targetDir = -collision.GetContact(0).normal;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            var rot = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            GameObject.Instantiate(_prefab, position, rot);
        }
    }
}
