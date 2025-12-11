using R3;
using R3.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class BrickPresenter : MonoBehaviour
    {
        [SerializeField]
        private int _initialHp = 1;
        [SerializeField]
        [Range(0, 10)]
        private int _powerUpSpawnOdds = 3;
        [SerializeField]
        private PowerUpPresenter[] _powerUpPrefabs;

        public void Init()
        {
            Brick = new Brick(_initialHp, _powerUpSpawnOdds);

            this
                .OnCollisionEnter2DAsObservable()
                .Select(collision => collision.collider.GetComponent<BallPresenter>().Ball)
                .Subscribe(ball => Brick.RespondToBallCollision.Execute(ball))
                .AddTo(this);

            Brick
                .CreatePowerUp
                .Subscribe(_ => InstantiatePowerUp())
                .AddTo(this);

            Brick
                .Active
                .Subscribe(value => gameObject.SetActive(value))
                .AddTo(this);
        }

        public Brick Brick { get; private set; }

        private void InstantiatePowerUp()
        {
            int randNum = RandomUtil.Random.Next(0, _powerUpPrefabs.Length);
            PowerUpPresenter prefab = _powerUpPrefabs[randNum];
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
