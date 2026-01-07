using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using PrimeTween;
using R3;
using System.Collections;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class LifeTracker : MonoBehaviour, IPostInject
    {
        private readonly ReactiveProperty<int> _numLives = new();

        [SerializeField]
        private int _initialLives = 3;

        [SerializeField]
        private Transform _playerTrfm;

        [SerializeField]
        private Ease _ease;

        [Inject]
        private GameEvents _gameEvents;

        [Inject]
        private LevelEvents _levelEvents;

        private float _startY;

        public ReadOnlyReactiveProperty<int> NumLives => _numLives;

        /// <summary>
        /// Value is the number of remaining lives.
        /// </summary>
        public Observable<int> LifeLost { get; private set; }

        private void Awake()
        {
            _startY = _playerTrfm.position.y;
            _numLives.Value = _initialLives;

            LifeLost = _numLives
                .Pairwise()
                .Where(static x => x.Current < x.Previous)
                .Select(static x => x.Current);
        }

        void IPostInject.PostInject()
        {
            ULog.Trace("LifeTracker: Subscribing to GameStarted event to reset lives.");
            _gameEvents.GameStarted
                .Subscribe(_ =>
                {
                    _numLives.Value = _initialLives;
                    _playerTrfm.SetY(_startY);
                })
                .AddTo(this);
        }

        public void AddLife()
        {
            _numLives.Value += 1;
        }

        public void RemoveLife()
        {
            if (_numLives.Value == 0)
            {
                return;
            }

            StartCoroutine(nameof(AnimateLifeLoss));
        }

        private IEnumerator AnimateLifeLoss()
        {
            _levelEvents.LevelReady.Value = false;

            yield return Tween
                .PositionY(_playerTrfm, _playerTrfm.position.y, _playerTrfm.position.y + 1f, 0.5f, _ease)
                .Chain(Tween.PositionY(_playerTrfm, _playerTrfm.position.y, _playerTrfm.position.y - 2f, 0.4f))
                .ToYieldInstruction();

            _numLives.Value -= 1;

            if (_numLives.Value == 0)
            {
                yield break;
            }

            yield return Tween
                .PositionY(_playerTrfm, _playerTrfm.position.y, _playerTrfm.position.y + 2f, 0.2f, startDelay: 0.8f)
                .ToYieldInstruction();

            _levelEvents.LevelReady.Value = true;
        }
    }
}
