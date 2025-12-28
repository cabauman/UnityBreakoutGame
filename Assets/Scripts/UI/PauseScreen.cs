using System;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;

namespace BreakoutGame
{
    public sealed class PauseScreen : MonoBehaviour
    {
        [Header("Object References")]

        [SerializeField]
        private Button _resumeButton;

        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private GameObject _screen;

        [SerializeField]
        private GameManager _gameManager;

        private void Start()
        {
            Ensure.NotNull(_resumeButton);
            Ensure.NotNull(_exitButton);
            Ensure.NotNull(_screen);
            Ensure.NotNull(_gameManager);

            _gameManager.IsPaused
                .Subscribe(isPaused => _screen.SetActive(isPaused))
                .AddTo(this);

            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnResumeButtonClicked()
        {
            _gameManager.IsPaused.Value = false;
        }

        private void OnExitButtonClicked()
        {
            ULog.Trace("Exit button clicked. Returning to main menu.");
        }
    }
}
