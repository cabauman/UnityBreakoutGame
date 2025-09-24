using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Paddle : MonoBehaviour
    {
        [Flatten]
        [SerializeField]
        private Config _config;

        public PaddlePresenter Presenter { get; set; }

        public Transform InitialBallPosTrfm => _config._initialBallPosTrfm;

        private void Awake()
        {
            Presenter = new PaddlePresenter(gameObject, _config);

            this
                .OnCollisionEnter2DAsObservable()
                .Subscribe(x => Presenter.OnCollisionEnter2D(x.gameObject, x.GetContact(0).point))
                .AddTo(this);
        }

        private void Update() => Presenter.Tick(Time.deltaTime);

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Paddle.OnCollisionEnter2D(collision);
        //}

        [Serializable]
        public sealed class Config
        {
            public GameObject _ballObj;
            public Transform _initialBallPosTrfm;
            public SpriteRenderer _spriteRenderer;
        }
    }
}