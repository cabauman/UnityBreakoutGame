using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public struct BallSpawnData
    {
        public Vector3 Position;
        public Transform Parent;
    }

    public sealed partial class MainBallSpawner : MonoBehaviour, IPostInject
    {
        [SerializeField]
        private Transform _spawnPoint;

        [Inject]
        private LevelEvents _levelEvents;

        [SerializeField]
        private BallManager _ballManager;

        [SerializeField]
        private LifeTracker _lifeTracker;

        private void Awake()
        {
            _lifeTracker.NumLives
                .Pairwise()
                .Where(static x => x.Current > 0 && x.Current < x.Previous)
                .Subscribe(_ =>
                {
                    var data = new BallSpawnData
                    {
                        Position = _spawnPoint.position,
                        Parent = _spawnPoint
                    };

                    ULog.Trace("Spawning ball.");
                    _ballManager.SpawnBallCmd.Execute(data);
                });
        }

        void IPostInject.PostInject()
        {
            ULog.Trace("Subscribing to LevelStarted event to spawn main ball.");
            _levelEvents.LevelStarted
                .Subscribe(_ =>
                {
                    var data = new BallSpawnData
                    {
                        Position = _spawnPoint.position,
                        Parent = _spawnPoint
                    };

                    ULog.Trace($"Spawning ball at {data.Position}");
                    _ballManager.SpawnBallCmd.Execute(data);
                });
        }
    }
}
