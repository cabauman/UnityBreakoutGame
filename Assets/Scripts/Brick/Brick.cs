using System;
using UniDig;
using R3;
using R3.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed partial class Brick : MonoBehaviour, GameCtor.DevToolbox.IPostInject
    {
        [Flatten]
        [SerializeField]
        private Config _config;

        [SerializeField]
        private PowerUpTable _powerUpTable;

        [Inject]
        private IPowerUpSpawner _powerUpSpawner;

        public BrickPresenter Presenter { get; private set; }

        private void Awake()
        {
            this
                .OnCollisionEnter2DAsObservable()
                .Subscribe(x => Presenter.OnCollisionEnter2D(x.gameObject))
                .AddTo(this);
            this
                .OnTriggerEnter2DAsObservable()
                .Subscribe(x => Presenter.OnCollisionEnter2D(x.gameObject))
                .AddTo(this);

            // TODO: Consider doing this in the bootstrapper instead
            GameCtor.DevToolbox.StartupLifecycle.AddPostInjectListener(PostInject);
        }

        public void PostInject()
        {
            Presenter = new BrickPresenter(gameObject, _config, _powerUpTable, _powerUpSpawner);
        }

        [Serializable]
        public sealed class Config
        {
            // TODO: Remove this field
            public int _initialHp = 1;
            public MonoCommand[] _hitCommands;
        }
    }
}