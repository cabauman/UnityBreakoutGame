using System;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using GameCtor.FuseDI;

namespace BreakoutGame
{
    public sealed partial class GameOverScreen : MonoBehaviour
    {
        [Header("Object References")]

        [SerializeField]
        private GameObject _screen;

        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private TextMeshProUGUI _gameWonLabel;

        [SerializeField]
        private TextMeshProUGUI _gameLostLabel;

        [SerializeField]
        private Button _playAgainButton;

        [Header("Parameters")]

        [SerializeField]
        private float _delayBeforeDisplayingPlayAgainButton = 3f;

        [Inject]
        private GameEvents _gameEvents;

        private void Start()
        {
            _gameEvents
                .GameWon
                .Subscribe(_ => _gameWonLabel.gameObject.SetActive(true))
                .AddTo(this);

            _gameEvents
                .GameLost
                .Subscribe(_ => _gameLostLabel.gameObject.SetActive(true))
                .AddTo(this);

            Observable
                .Merge(_gameEvents.GameWon, _gameEvents.GameLost)
                .Subscribe(_ => _screen.SetActive(true))
                .AddTo(this);

            Observable
                .Merge(_gameEvents.GameWon, _gameEvents.GameLost)
                .Delay(TimeSpan.FromSeconds(_delayBeforeDisplayingPlayAgainButton))
                .Subscribe(_ => _playAgainButton.gameObject.SetActive(true))
                .AddTo(this);

            _playAgainButton.OnClickAsObservable()
                .Subscribe(_ => OnPlayAgainClicked())
                .AddTo(this);
        }

        private void OnPlayAgainClicked()
        {
            _gameManager.Play().Forget();
            HideUI();
        }

        private void HideUI()
        {
            _screen.SetActive(false);
            _gameWonLabel.gameObject.SetActive(false);
            _gameLostLabel.gameObject.SetActive(false);
            _playAgainButton.gameObject.SetActive(false);
        }
    }
}
