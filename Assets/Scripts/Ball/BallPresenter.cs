using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BallPresenter
    {
        private readonly GameObject _view;
        private readonly Ball.Config _config;
        private readonly Rigidbody2D _rigidbody;

        public BallPresenter(GameObject view, Ball.Config config)
        {
            _view = view;
            _config = config;
            InitialForce = config._initialForce;
            Power = config._power;
            StartPosition = Vector3.zero;
            Active = new ReactiveProperty<bool>(true);

            _rigidbody = view.GetComponent<Rigidbody2D>();

            Active
                .Subscribe(value => view.SetActive(value));
        }

        public float InitialForce { get; }

        public int Power { get; }

        public Vector3 StartPosition { get; }

        public IReactiveProperty<bool> Active { get; }

        public void AddInitialForce()
        {
            _view.transform.parent = null;
            var force = new Vector2
            {
                x = Mathf.Sin(_config._initialAngle * Mathf.Deg2Rad) * InitialForce,
                y = Mathf.Cos(_config._initialAngle * Mathf.Deg2Rad) * InitialForce
            };
            _rigidbody.AddForce(force);
        }

        public void SetForce(Vector2 force)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(force);
        }

        public void OnTriggerEnter2D(GameObject other)
        {
            if (other.CompareTag(Tags.DEAD_ZONE))
            {
                Active.Value = false;
            }
        }
    }
}