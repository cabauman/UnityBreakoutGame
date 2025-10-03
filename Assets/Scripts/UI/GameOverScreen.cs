using System;
using TMPro;
using UniDig;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutGame
{
    public partial class GameOverScreen : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField]
        private TextMeshProUGUI _gameWonLabel;
        [SerializeField]
        private TextMeshProUGUI _gameLostLabel;
        [SerializeField]
        private Button _playAgainButton;

        [Header("Parameters")]
        [SerializeField]
        private float _delayBeforeDisplayingPlayAgainButton = 3f;

        [Inject] private Game _game;

        private void Start()
        {
            _game
                .GameWon
                .Subscribe(_ => _gameWonLabel.gameObject.SetActive(true))
                .AddTo(this);

            _game
                .GameLost
                .Subscribe(_ => _gameLostLabel.gameObject.SetActive(true))
                .AddTo(this);

            _game.GameWon.Merge(_game.GameLost)
                .Delay(TimeSpan.FromSeconds(_delayBeforeDisplayingPlayAgainButton))
                .Subscribe(_ => _playAgainButton.gameObject.SetActive(true))
                .AddTo(this);

            _game.ResetGameCmd.BindToOnClick(_playAgainButton, _ => HideUI());
        }

        private void HideUI()
        {
            _gameWonLabel.gameObject.SetActive(false);
            _gameLostLabel.gameObject.SetActive(false);
            _playAgainButton.gameObject.SetActive(false);
        }
    }
}