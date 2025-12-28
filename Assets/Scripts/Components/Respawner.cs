using GameCtor.DevToolbox;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Respawner : MonoBehaviour
    {
        [SerializeField]
        private IReadOnlyLevelEvents _levelEvents;

        [SerializeField]
        private BallManager _ballManager;

        [SerializeField]
        private LifeTracker _lifeTracker;

        private void Awake()
        {
            //_levelEvents.LifeLost
            //    .Where(_ => _lifeTracker.NumLives.CurrentValue > 0)
            //    .Subscribe(_ => _ballManager.RespawnBall())
            //    .AddTo(this);

            //_lifeLostObservable = _lifeTracker.NumLives
            //    .Pairwise()
            //    .Where(static x => x.Current < x.Previous)
            //    .SelectAwait(async (x, ct) => { await _lifeLostSequence.Execute(); return x.Current; })
            //    .TakeUntil(_isQuitting.Skip(1));

            //_lifeLostObservable
            //    .SubscribeAwait(async (numLives, ct) =>
            //    {
            //        if (numLives > 0)
            //        {
            //            ULog.Trace("Starting respawn sequence");
            //            await _respawnSequence.Execute();
            //            _levelStarted.OnNext(Unit.Default); // or _playerReady/_levelReady
            //            ULog.Trace("Respawn sequence complete, ball respawned");
            //        }
            //    });

            //_levelStarted
            //    .Subscribe(_ =>
            //    {
            //        var data = new BallSpawnData
            //        {
            //            Position = _ballSpawnPoint.position,
            //            Parent = _ballSpawnPoint
            //        };

            //        ULog.Trace($"Spawning ball at {data.Position}");
            //        _ballManager.SpawnBallCmd.Execute(data);
            //    });
        }
    }
}
