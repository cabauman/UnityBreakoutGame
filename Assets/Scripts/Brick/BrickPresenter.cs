using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BrickPresenter : MonoBehaviour
    {
        [SerializeField]
        private Config _config;

        private void Awake()
        {
            Brick = new Brick(gameObject, _config);
        }

        public Brick Brick { get; private set; }

        public sealed class Config
        {
            public int _initialHp = 1;
            [Range(0, 10)]
            public int _powerUpSpawnOdds = 3;
            public PowerUpPresenter _powerUpPrefab;
        }
    }
}