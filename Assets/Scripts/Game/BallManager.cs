using GameCtor.DevToolbox;
using R3;
using R3.Triggers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BreakoutGame
{
    public sealed class BallManager : MonoBehaviour
    {
        private readonly List<Ball> _balls = new();

        [SerializeField]
        private Ball _ballPrefab;

        [SerializeField]
        private LifeTracker _lifeTracker;

        private ObjectPool<Ball> _ballPool;
        private bool _isQuitting = false;

        public ReadOnlyReactiveProperty<int> NumBallsInPlay { get; private set; }

        public ReactiveCommand<BallSpawnData> SpawnBallCmd { get; private set; } = new();

        public IReadOnlyList<Ball> Balls => _balls;

        // TODO: Remove if not used
        public Ball Spawn(Vector3 position)
        {
            var ball = _ballPool.Get();
            ball.transform.position = position;
            return ball;
        }

        public void ResetAll()
        {
            for (int i = _balls.Count - 1; i >= 0; --i)
            {
                var ball = _balls[i];
                if (ball != null)
                {
                    ball.gameObject.SetActive(false);
                }
            }
        }

        private static Observable<Unit> DetectWhenBallBecomesInactive(Ball ball)
        {
            return ball
                .OnDisableAsObservable()
                .Do(ball, static (_, b) => b.ReleaseToPool())
                .Take(1);
        }

        private void Awake()
        {
            _ballPool = new ObjectPool<Ball>(
                createFunc: () =>
                {
                    var instance = Instantiate(_ballPrefab);
                    instance.gameObject.SetActive(false);
                    instance.SetOwningPool(_ballPool);
                    return instance;
                },
                actionOnGet: ball =>
                {
                    ball.gameObject.SetActive(true);
                    _balls.Add(ball);
                },
                actionOnRelease: ball => _balls.Remove(ball),
                actionOnDestroy: ball =>
                {
                    if (ball != null) Destroy(ball.gameObject);
                },
                collectionCheck: true,
                defaultCapacity: 1,
                maxSize: 5);

            var o1 = SpawnBallCmd.Select(SpawnBall).Publish().RefCount();
            var o2 = o1.SelectMany(DetectWhenBallBecomesInactive);

            NumBallsInPlay = Observable.Merge(
                    o1.Select(static _ => 1),
                    o2.Select(static _ => -1))
                .Scan(static (acc, val) => acc + val)
                .Where(_ => !_isQuitting)
                .ToReadOnlyReactiveProperty(1);
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private Ball SpawnBall(BallSpawnData data)
        {
            ULog.Trace($"Position: {data.Position}");
            var ball = _ballPool.Get();
            ball.transform.position = data.Position;
            ball.transform.SetParent(data.Parent, true);
            return ball;
        }
    }
}
