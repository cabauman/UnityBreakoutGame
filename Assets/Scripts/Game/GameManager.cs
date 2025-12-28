using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    [Serializable]
    public class LevelData
    {
        public int version = 1;
        public int level;
        public List<BrickData> bricks = new();
    }

    [Serializable]
    public class BrickData
    {
        // Addressables key, e.g. "brick.basic.red"
        public string type;
        public AssetReferenceGameObject prefab;
        public int x;
        public int y;
    }

    public interface IReadOnlyGameState
    {
        ReadOnlyReactiveProperty<int> NumLives { get; }
        ReadOnlyReactiveProperty<int> Level { get; }
    }

    public interface IReadOnlyPlayerEvents
    {
        Observable<Unit> LifeLost { get; }
        Observable<Unit> PlayerSpawned { get; }
    }

    public interface IReadOnlyGameEvents
    {
        Observable<Unit> GameWon { get; }
        Observable<Unit> GameLost { get; }
    }

    public interface IReadOnlyLevelEvents
    {
        Observable<Unit> LevelReady { get; }
        Observable<Unit> LevelStarted { get; }
        Observable<int> LevelPassed { get; }
        Observable<Unit> LevelFailed { get; }
        Observable<Unit> LevelPaused { get; }
    }

    public sealed class GameEvents : IReadOnlyGameEvents
    {
        public Subject<Unit> GameWon { get; } = new();
        public Subject<Unit> GameLost { get; } = new();

        Observable<Unit> IReadOnlyGameEvents.GameWon => GameWon;
        Observable<Unit> IReadOnlyGameEvents.GameLost => GameLost;
    }

    public sealed class LevelEvents : IReadOnlyLevelEvents
    {
        public Subject<Unit> LevelReady { get; } = new();
        public Subject<Unit> LevelStarted { get; } = new();
        public Subject<int> LevelPassed { get; } = new();
        public Subject<Unit> LevelFailed { get; } = new();
        public Subject<Unit> LevelPaused { get; } = new();

        Observable<Unit> IReadOnlyLevelEvents.LevelReady => LevelReady;
        Observable<Unit> IReadOnlyLevelEvents.LevelStarted => LevelStarted;
        Observable<int> IReadOnlyLevelEvents.LevelPassed => LevelPassed;
        Observable<Unit> IReadOnlyLevelEvents.LevelFailed => LevelFailed;
        Observable<Unit> IReadOnlyLevelEvents.LevelPaused => LevelPaused;
    }

    public class AsyncSequence
    {
        public Task<Unit> Execute() => Observable.Timer(TimeSpan.FromSeconds(1)).FirstAsync();
    }

    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelData[] _levelData;
        [SerializeField] private LevelManager _levelManager;

        private readonly Subject<Unit> _gameStarted = new();
        private readonly Subject<bool> _paused = new();
        private readonly Subject<Unit> _gameWon = new();
        private readonly Subject<Unit> _gameLost = new();

        public Observable<Unit> GameStarted => _gameStarted;
        public Observable<bool> Paused => _paused;
        public Observable<Unit> GameWon => _gameWon;
        public Observable<Unit> GameLost => _gameLost;

        public ReactiveProperty<int> Level { get; private set; } = new();
        public ReactiveProperty<bool> IsPaused { get; private set; } = new();

        private IEnumerator Start()
        {
            yield return null;
            //var json = JsonUtility.ToJson(_levelData[0]);
            //Debug.Log(json);
            Play().Forget();

            IsPaused
                .Skip(1)
                .Subscribe(paused =>
                {
                    Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
                    _paused.OnNext(Time.timeScale == 0f);
                })
                .AddTo(this);
        }

        private void Update()
        {
            if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
            {
                IsPaused.Value = !IsPaused.Value;
            }
        }

        public async UniTask Play()
        {
            Level.Value = 0;
            _gameStarted.OnNext(Unit.Default);

            foreach (var levelDataItem in _levelData)
            {
                Level.Value += 1;
                var result = await _levelManager.Play(levelDataItem);

                if (result.Type == LevelResultType.None)
                {
                    return;
                }

                if (result.Type == LevelResultType.Lost)
                {
                    _gameLost.OnNext(Unit.Default);
                    return;
                }
            }

            _gameWon.OnNext(Unit.Default);
        }
    }
}
