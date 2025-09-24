using System;
using UniDig;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed partial class Brick : MonoBehaviour
    {
        [Flatten]
        [SerializeField]
        private Config _config;

        [Inject] private IPowerUpSpawner _powerUpSpawner;

        private void Awake()
        {
            Presenter = new BrickPresenter(gameObject, _config, _powerUpSpawner);

            this
                .OnCollisionEnter2DAsObservable()
                .Select(collision => collision.gameObject)
                .Subscribe(go => Presenter.OnCollisionEnter2D(go))
                .AddTo(this);
        }

        public BrickPresenter Presenter { get; private set; }

        [Serializable]
        public sealed class Config
        {
            // TODO: Naming
            public int _initialHp = 1;
        }
    }
}