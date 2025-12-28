using PrimeTween;
using R3;
using System.Collections;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class LifeTracker : MonoBehaviour
    {
        private readonly ReactiveProperty<int> _numLives = new();

        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private int _initialLives = 3;

        [SerializeField]
        private Transform _playerTrfm;

        [SerializeField]
        private Ease _ease;

        public ReadOnlyReactiveProperty<int> NumLives => _numLives;

        public Observable<int> LifeLost { get; private set; }

        private void Awake()
        {
            _numLives.Value = _initialLives;
            _gameManager.GameStarted.Subscribe(_ => _numLives.Value = _initialLives);

            LifeLost = _numLives
                .Pairwise()
                .Where(static x => x.Current < x.Previous)
                .Select(static x => x.Current);
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
            //_numLives.Value -= 1;
            StartCoroutine(nameof(AnimateLifeLoss));
        }

        private IEnumerator AnimateLifeLoss()
        {
            yield return Tween
                .PositionY(_playerTrfm, _playerTrfm.position.y, _playerTrfm.position.y + 1f, 0.5f, _ease)
                .Chain(Tween.PositionY(_playerTrfm, _playerTrfm.position.y, _playerTrfm.position.y - 2f, 0.4f))
                .ToYieldInstruction();
            _numLives.Value -= 1;
            yield return Tween.PositionY(_playerTrfm, _playerTrfm.position.y, _playerTrfm.position.y + 2f, 0.2f, startDelay: 0.8f)
                .ToYieldInstruction();
        }
    }
}
