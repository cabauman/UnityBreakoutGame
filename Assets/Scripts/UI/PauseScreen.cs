using R3;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;
using GameCtor.FuseDI;

namespace BreakoutGame
{
    public sealed partial class PauseScreen : MonoBehaviour, IPostInject
    {
        [Header("Object References")]

        [SerializeField]
        private Button _resumeButton;

        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private GameObject _screen;

        [Inject]
        private GameEvents _gameEvents;

        private void Start()
        {
            ULog.Trace("");
            Ensure.NotNull(_resumeButton);
            Ensure.NotNull(_exitButton);
            Ensure.NotNull(_screen);
            Ensure.NotNull(_gameEvents);

            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        void IPostInject.PostInject()
        {
            ULog.Trace("");

            _gameEvents.IsPaused
                .Subscribe(isPaused => _screen.SetActive(isPaused))
                .AddTo(this);
        }

        private void OnResumeButtonClicked()
        {
            _gameEvents.IsPaused.Value = false;
        }

        private void OnExitButtonClicked()
        {
            ULog.Trace("Exit button clicked. Returning to main menu.");
        }
    }
}
