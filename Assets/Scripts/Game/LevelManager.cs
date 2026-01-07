using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using System.Linq;
using UnityEngine;

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
        public int Level;
        public LevelResultType Type;
    }

    public sealed partial class LevelManager : MonoBehaviour
    {
        private readonly Subject<Unit> _quitting = new();

        [SerializeField]
        private BrickManager _brickManager;

        [SerializeField]
        private LifeTracker _lifeTracker;

        [SerializeField]
        private LevelTransitioner _levelTransitioner;

        [Inject]
        private LevelEvents _levelEvents;

        private int _currentLevel;

        public async UniTask Initialize(LevelData levelData)
        {
            _currentLevel = levelData.level;
            await _brickManager.LoadLevel(levelData);
            await _levelTransitioner.Enter();
        }

        public async UniTask<LevelResult> Play()
        {
            ULog.Trace($"LevelManager: Starting level {_currentLevel}.");
            _levelEvents.LevelStarted.OnNext(_currentLevel);

            var result = await Observable.Race(
                    AllBricksDestroyed(),
                    NoLivesRemaining(),
                    IsQuitting())
                .FirstAsync();

            return result;
        }

        public async UniTask Finalize(LevelResult result)
        {
            await _levelTransitioner.Exit(result);
        }

        private void OnApplicationQuit()
        {
            _quitting.OnNext(Unit.Default);
        }

        // TODO: Need to also handle exiting via pause menu
        private Observable<LevelResult> IsQuitting()
        {
            return _quitting
                .Select(_currentLevel, static (_, level) => new LevelResult { Level = level, Type = LevelResultType.None });
        }

        private Observable<LevelResult> NoLivesRemaining()
        {
            return _lifeTracker.LifeLost
                .Where(static x => x == 0)
                .Do(x => _levelEvents.LevelFailed.OnNext(_currentLevel))
                .Select(_currentLevel, static (_, level) => new LevelResult { Level = level, Type = LevelResultType.Lost });
        }

        private Observable<LevelResult> AllBricksDestroyed()
        {
            return _brickManager.NumBricks
                .Where(static numBricks => numBricks == 0)
                .Do(x => _levelEvents.LevelPassed.OnNext(_currentLevel))
                .Select(_currentLevel, static (_, level) => new LevelResult { Level = level, Type = LevelResultType.Won });
        }
    }
}
