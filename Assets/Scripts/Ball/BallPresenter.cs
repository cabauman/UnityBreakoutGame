using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BallPresenter
    {
        private readonly GameObject _view;
        private readonly Ball.Config _config;
        private readonly Rigidbody2D _rigidbody;

        public Transform Trfm => _view.transform;

        public int Power { get; }

        public IReactiveProperty<bool> Active { get; }

        public BallPresenter(GameObject view, Ball.Config config)
        {
            _view = view;
            _config = config;
            Power = config._power;
            Active = new ReactiveProperty<bool>(true);

            _rigidbody = view.GetComponent<Rigidbody2D>();

            Active
                .Subscribe(value => view.SetActive(value));
        }

        //public void AddInitialForce()
        //{
        //    _view.transform.parent = null;
        //    var force = new Vector2
        //    {
        //        x = Mathf.Sin(_config._initialAngle * Mathf.Deg2Rad) * InitialForce,
        //        y = Mathf.Cos(_config._initialAngle * Mathf.Deg2Rad) * InitialForce
        //    };
        //    _rigidbody.AddForce(force);
        //}

        public void SetForce(Vector2 force)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public void OnTriggerEnter2D(GameObject other)
        {
            if (other.CompareTag(Tags.DEAD_ZONE))
            {
                Active.Value = false;
            }
        }

        public void AttachTo(Transform transform)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _view.transform.SetParent(transform);
        }
    }
}