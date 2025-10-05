using System;
using R3;
using R3.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public sealed class Ball : MonoBehaviour, IView, ITestable<Ball.Config>
    {
        [Flatten]
        [SerializeField]
        private Config _config;

        private void Awake()
        {
            Presenter = new BallPresenter(gameObject, _config);

            this
                .OnTriggerEnter2DAsObservable()
                .Subscribe(x => Presenter.OnTriggerEnter2D(x.gameObject))
                .AddTo(this);
        }

        void ITestable<Config>.SetConfig(Config config) => _config = config;

        public BallPresenter Presenter { get; set; }

        public GameObject GameObject => gameObject;

        [Serializable]
        public sealed class Config
        {
            //[Range(0f, 100f)]
            //public float _initialForce = 50f;

            //// TODO: Rename to _initialAngleDeg
            //[Range(-90f, 90f)]
            //public float _initialAngle = 45f;

            [Tooltip("How much damage I can inflict on a brick per collision.")]
            public int _power = 1;

            //// TODO: Move to paddle config
            //[Range(0f, 90f)]
            //[Tooltip("0: completely vertical, 90: +/- 90degrees from the up vector")]
            //public float _maxPaddleBounceAngle = 75f;
        }
    }
}