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

        public BrickPresenter Presenter { get; private set; }

        private void Awake()
        {
            //Debug.Log("Brick Awake");
            this
                .OnCollisionEnter2DAsObservable()
                .Subscribe(x => Presenter.OnCollisionEnter2D(x.gameObject))
                .AddTo(this);
            GameCtor.DevToolbox.StartupLifecycle.AddPostInjectListener(PostInject);
        }

        public void PostInject()
        {
            Presenter = new BrickPresenter(gameObject, _config, _powerUpSpawner);
        }

        [Serializable]
        public sealed class Config
        {
            // TODO: Naming
            public int _initialHp = 1;
        }
    }
}