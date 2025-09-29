using System;
using UniDig;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed partial class Brick : MonoBehaviour, GameCtor.DevToolbox.IPostInject
    {
        [Flatten]
        [SerializeField]
        private Config _config;

        [Inject] private IPowerUpSpawner _powerUpSpawner;

        private void Awake()
        {
            //Debug.Log("Brick Awake");
            this
                .OnCollisionEnter2DAsObservable()
                .Subscribe(x => Presenter.OnCollisionEnter2D(x.gameObject))
                .AddTo(this);
            GameCtor.DevToolbox.StartupLifecycle.AddPostInjectListener(PostInject);
        }

        //private void Start()
        //{
        //    Debug.Log("Brick Start");
        //}

        public void PostInject()
        {
            Presenter = new BrickPresenter(gameObject, _config, _powerUpSpawner);
        }

        //public void Inject(BreakoutGame.IPowerUpSpawner _powerUpSpawner)
        //{
        //    Debug.Log("Brick Inject");
        //    this._powerUpSpawner = _powerUpSpawner;
        //    Presenter = new BrickPresenter(gameObject, _config, _powerUpSpawner);

        //    this
        //        .OnCollisionEnter2DAsObservable()
        //        .Select(collision => collision.gameObject)
        //        .Subscribe(go => Presenter.OnCollisionEnter2D(go))
        //        .AddTo(this);
        //}

        public BrickPresenter Presenter { get; private set; }

        [Serializable]
        public sealed class Config
        {
            // TODO: Naming
            public int _initialHp = 1;
        }
    }
}