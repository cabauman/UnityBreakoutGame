using GameCtor.DevToolbox;
using R3;
using System;
using UnityEngine;

namespace BreakoutGame
{
    public struct BallSpawnData
    {
        public Vector3 Position;
        public Transform Parent;
    }

    public sealed class MainBallSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private LifeTracker _lifeTracker;

        [SerializeField]
        private BallManager _ballManager;

        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private LevelManager _levelManager;

        //private void Awake()
        //{
        //    return;
        //    _lifeTracker.NumLives
        //        .Pairwise()
        //        .Where(static x => x.Current > 0 && x.Current < x.Previous)
        //        .AsUnitObservable()
        //        .Delay(TimeSpan.FromSeconds(1))
        //        .Merge(_levelManager.Started)
        //        //.Prepend(Unit.Default)
        //        .Subscribe(_ =>
        //        {
        //            ULog.Trace("MainBallSpawner: Spawning ball.");
        //            var data = new BallSpawnData
        //            {
        //                Position = _spawnPoint.position,
        //                Parent = _spawnPoint
        //            };
        //            _ballManager.SpawnBallCmd.Execute(data);
        //        });
        //}
    }
}
