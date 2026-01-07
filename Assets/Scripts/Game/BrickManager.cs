using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;
using GameCtor.FuseDI;
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
    public struct BrickItem
    {
        public ObjectPool<GameObject> Pool;
        public AsyncOperationHandle<GameObject> Handle;
    }

    public sealed partial class BrickManager : MonoBehaviour
    {
        private readonly Dictionary<string, BrickItem> _loadedBricks = new();
        private readonly List<GameObject> _instantiatedBricks = new();

        [SerializeField]
        private Transform _brickParent;

        [SerializeField]
        private Vector2 _cellSize = new(1.711f, 0.4f);

        [Inject]
        private Injector _injector;

        public ReadOnlyReactiveProperty<int> NumBricks { get; private set; }

        public async UniTask LoadLevel(LevelData level, CancellationToken ct = default)
        {
            foreach (var instance in _instantiatedBricks)
            {
                if (instance.activeSelf)
                {
                    instance.SetActive(false);
                }
            }

            _instantiatedBricks.Clear();

            var brickTypes = level.bricks.Select(static x => x.type).ToHashSet();
            var brickTypesToUnload = _loadedBricks.Keys.Except(brickTypes).ToArray();

            foreach (var brickType in brickTypesToUnload)
            {
                if (_loadedBricks.TryGetValue(brickType, out var item))
                {
                    item.Pool.Dispose();
                    Addressables.Release(item.Handle);
                    _loadedBricks.Remove(brickType);
                }
            }

            foreach (var brick in level.bricks)
            {
                await SpawnBricks(brick, ct);
            }

            var destructableBricks = _instantiatedBricks
                .Where(x => x.TryGetComponent<Health>(out _))
                .ToArray();

            NumBricks = destructableBricks.ToObservable()
                .Where(static x => x.TryGetComponent<Health>(out _))
                .SelectMany(static x => x.OnDisableAsObservable().Select(y => -1))
                .Scan(destructableBricks.Length, static (acc, delta) => acc + delta)
                .ToReadOnlyReactiveProperty(destructableBricks.Length);
        }

        private async UniTask SpawnBricks(BrickData brick, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (!_loadedBricks.TryGetValue(brick.type, out var item))
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(brick.type);
                await handle.ToUniTask(cancellationToken: ct);

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    ULog.Error($"Failed to load brick type '{brick.type}'");
                    return;
                }

                item = new BrickItem();
                var pool = new ObjectPool<GameObject>(
                    createFunc: () =>
                    {
                        var instance = _injector.Instantiate(handle.Result, Vector3.zero, Quaternion.identity, _brickParent);
                        instance.OnDisableAsObservable()
                            .Subscribe(_ =>
                            {
                                item.Pool.Release(instance);
                            })
                            .AddTo(instance);
                        return instance;
                    },
                    actionOnRelease: obj => { },
                    actionOnGet: obj => obj.SetActive(true),
                    actionOnDestroy: obj => { if (obj != null) Destroy(obj); },
                    collectionCheck: true,
                    defaultCapacity: 10,
                    maxSize: 50);

                _loadedBricks[brick.type] = item = new BrickItem
                {
                    Pool = pool,
                    Handle = handle,
                };
            }

            var worldPos = GridToWorld(brick.x, brick.y);
            var instance = item.Pool.Get();
            instance.transform.position = worldPos;
            _instantiatedBricks.Add(instance);
        }

        private Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x * _cellSize.x, y * _cellSize.y, 0f);
        }
    }
}
