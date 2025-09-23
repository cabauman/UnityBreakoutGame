using System;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class Brick : MonoBehaviour
    {
        [MyConfig]
        [SerializeField]
        private Config _config;

        private void Awake()
        {
            Presenter = new BrickPresenter(gameObject, _config);
        }

        public BrickPresenter Presenter { get; private set; }

        [Serializable]
        public sealed class Config
        {
            public int _initialHp = 1;
            [Range(0, 10)]
            public int _powerUpSpawnOdds = 3;
            public PowerUp _powerUpPrefab;
        }
    }
}