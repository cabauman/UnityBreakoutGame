using System;
using UniDig;
using R3;
using R3.Triggers;
using UnityEngine;
using GameCtor.FuseDI;

namespace BreakoutGame
{
    public sealed partial class Paddle : MonoBehaviour, IPostInject
    {
        [Flatten]
        [SerializeField]
        private Config _config;

        [Inject] private IBallPaddleCollisionStrategy _collisionStrategy;

        public PaddlePresenter Presenter { get; set; }

        public Transform InitialBallPosTrfm => _config._initialBallPosTrfm;

        private void Awake()
        {
            _config._spriteRenderer
                .OnCollisionEnter2DAsObservable()
                .Subscribe(x => Presenter.OnCollisionEnter2D(x.gameObject, x.GetContact(0).point))
                .AddTo(this);

            GameCtor.DevToolbox.StartupLifecycle.AddPostInjectListener(PostInject);
        }

        public void PostInject()
        {
            Debug.Log("Paddle PostInject");
            Presenter = new PaddlePresenter(gameObject, _collisionStrategy, _config);
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Presenter.OnCollisionEnter2D(collision.gameObject, collision.GetContact(0).point);
        //}

        [Serializable]
        public sealed class Config
        {
            public Ball _ballObj;
            public Transform _initialBallPosTrfm;
            public SpriteRenderer _spriteRenderer;
            public GameEvent _gameplayStartedEvent;

            [Range(0f, 100f)]
            public float _ballLaunchForce = 50f;

            public float _maxBounceAngleDeg = 75f;
            public float MaxBounceAngleRad => _maxBounceAngleDeg * Mathf.Deg2Rad;
        }
    }
}