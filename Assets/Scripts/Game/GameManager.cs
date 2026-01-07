using Cysharp.Threading.Tasks;
using GameCtor.FuseDI;
using R3;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public sealed partial class GameManager : MonoBehaviour
    {
        [SerializeField]
        private LevelData[] _levelData;

        [SerializeField]
        private LevelManager _levelLifecycle;

        [Inject]
        private GameEvents _gameEvents;

        private bool _levelInProgress;

        public ReactiveProperty<int> Level { get; private set; } = new();

        private IEnumerator Start()
        {
            yield return null;

            Play().Forget();

            _gameEvents.IsPaused
                .Skip(1)
                .Subscribe(paused =>
                {
                    Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
                })
                .AddTo(this);
        }

        private void Update()
        {
            if (!_levelInProgress)
            {
                return;
            }

            if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
            {
                _gameEvents.IsPaused.Value = !_gameEvents.IsPaused.Value;
            }
        }

        public async UniTask Play()
        {
            Level.Value = 0;
            _gameEvents.GameStarted.OnNext(Unit.Default);

            foreach (var levelDataItem in _levelData)
            {
                Level.Value += 1;

                _levelInProgress = true;
                await _levelLifecycle.Initialize(levelDataItem);
                var result = await _levelLifecycle.Play();
                await _levelLifecycle.Finalize(result);
                _levelInProgress = false;

                if (result.Type == LevelResultType.None)
                {
                    return;
                }

                if (result.Type == LevelResultType.Lost)
                {
                    _gameEvents.GameLost.OnNext(Unit.Default);
                    return;
                }
            }

            _gameEvents.GameWon.OnNext(Unit.Default);
        }
    }
}
