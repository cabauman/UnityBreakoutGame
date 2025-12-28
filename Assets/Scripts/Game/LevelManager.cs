using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;
using R3;
using R3.Triggers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BreakoutGame
{
    public enum LevelResultType
    {
        None,
        Won,
        Lost
    }
    public struct LevelResult
    {
        public LevelResultType Type;
    }

    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private BrickManager _brickManager;
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private LifeTracker _lifeTracker;
        [SerializeField] private Transform _ballSpawnPoint;

        [SerializeField] private Transform _brickParent;
        [SerializeField] private Vector2 _cellSize = new(1.711f, 0.4f);
        private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _loadedBricks = new();
        private readonly List<GameObject> _instantiatedBricks = new();

        private AsyncSequence _levelInitSequence = new();
        private AsyncSequence _levelPassedSequence = new();
        private Observable<int> _lifeLostObservable;
        private int _currentLevel;

        private readonly Subject<int> _levelStarted = new();
        private readonly ReactiveProperty<bool> _isQuitting = new(false);

        public Observable<int> LevelStarted => _levelStarted;
        public Subject<int> LevelPassed { get; } = new();
        public Subject<int> LevelFailed { get; } = new();

        public async UniTask<LevelResult> Play(LevelData levelData)
        {
            _currentLevel = levelData.level;
            await LoadFromJsonAsync(levelData);
            await _levelInitSequence.Execute();
            _levelStarted.OnNext(_currentLevel);

            var result = await Observable.Race(
                    AllBricksDestroyed(),
                    NoLivesRemaining(),
                    IsQuitting())
                .FirstAsync();

            if (_isQuitting.Value)
            {
                return result;
            }

            _ballManager.ResetAll();
            await _levelPassedSequence.Execute();

            return result;
        }

        // TODO: Need to also handle exiting via pause menu
        private Observable<LevelResult> IsQuitting()
        {
            return _isQuitting
                .Where(x => x == true)
                .Select(static x => new LevelResult { Type = LevelResultType.None });
        }

        private Observable<LevelResult> NoLivesRemaining()
        {
            return _lifeLostObservable
                .Where(_lifeTracker, static (x, lifeTracker) => lifeTracker.NumLives.CurrentValue == 0)
                .Do(x => LevelFailed.OnNext(_currentLevel))
                .Select(static x => new LevelResult { Type = LevelResultType.Lost });
        }

        private Observable<LevelResult> AllBricksDestroyed()
        {
            return _instantiatedBricks
                .ToObservable()
                .SelectMany(static x => x.OnDisableAsObservable().Select(static y => -1))
                .Scan(_instantiatedBricks.Count, static (acc, delta) => acc + delta)
                .Where(static numBricks => numBricks == 0)
                .Do(x => LevelPassed.OnNext(_currentLevel))
                .Select(static x => new LevelResult { Type = LevelResultType.Won });
        }

        private ObjectPool<GameObject> _brickPool;

        public async UniTask LoadFromJsonAsync(LevelData level, CancellationToken ct = default)
        {
            //var brickTypes = level.bricks.Select(static x => x.type).ToHashSet();
            //var brickTypesToUnload = _loadedBricks.Keys.Except(brickTypes);

            //foreach (var instance in _instantiatedBricks)
            //{
            //    Destroy(instance);
            //}

            //foreach (var brickType in brickTypesToUnload)
            //{
            //    if (_loadedBricks.TryGetValue(brickType, out var handle))
            //    {
            //        Addressables.Release(handle);
            //        _loadedBricks.Remove(brickType);
            //    }
            //}

            foreach (var brick in level.bricks)
            {
                ct.ThrowIfCancellationRequested();

                if (!_loadedBricks.TryGetValue(brick.type, out var handle))
                {
                    handle = Addressables.LoadAssetAsync<GameObject>(brick.type);
                    //handles.Add(handle);
                    await handle.ToUniTask(cancellationToken: ct);

                    if (handle.Status != AsyncOperationStatus.Succeeded)
                    {
                        ULog.Error($"Failed to load brick type '{brick.type}'");
                        continue;
                    }

                    _loadedBricks[brick.type] = handle;
                }

                var worldPos = GridToWorld(brick.x, brick.y);
                var instance = Instantiate(handle.Result, worldPos, Quaternion.identity, _brickParent);
                _instantiatedBricks.Add(instance);
            }
        }

        private Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x * _cellSize.x, y * _cellSize.y, 0f);
        }

        private void Start()
        {
            _lifeLostObservable = _lifeTracker.NumLives
                .Pairwise()
                .Where(static x => x.Current < x.Previous)
                .SelectAwait(async (x, ct) =>
                {
                    //await _lifeLostSequence.Execute();
                    return x.Current;
                });

            _lifeLostObservable
                .SubscribeAwait(async (numLives, ct) =>
                {
                    if (numLives > 0)
                    {
                        ULog.Trace("Starting respawn sequence");
                        //await _respawnSequence.Execute();
                        _levelStarted.OnNext(_currentLevel); // or _playerReady/_levelReady
                        ULog.Trace("Respawn sequence complete, ball respawned");
                    }
                });

            _ballManager.NumBallsInPlay
                .Skip(1)
                .Where(x => x == 0 && _instantiatedBricks.Any(static brick => brick.activeSelf))
                .TakeUntil(_isQuitting.Skip(1))
                .Subscribe(_ => _lifeTracker.RemoveLife());

            _levelStarted
                .Subscribe(_ =>
                {
                    var data = new BallSpawnData
                    {
                        Position = _ballSpawnPoint.position,
                        Parent = _ballSpawnPoint
                    };

                    ULog.Trace($"Spawning ball at {data.Position}");
                    _ballManager.SpawnBallCmd.Execute(data);
                });
        }

        private void OnApplicationQuit()
        {
            _isQuitting.Value = true;
        }

        private void OnDisable()
        {
            foreach (var instance in _instantiatedBricks)
            {
                if (instance != null)
                {
                    Destroy(instance);
                }
            }

            _instantiatedBricks.Clear();

            foreach (var handle in _loadedBricks.Values)
            {
                Addressables.Release(handle);
            }

            _loadedBricks.Clear();
        }
    }
}
