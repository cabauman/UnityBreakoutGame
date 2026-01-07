using R3;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using System.Collections;

namespace BreakoutGame
{
    public sealed class MainScreen : MonoBehaviour
    {
        [Header("Object References")]

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _bestTimesButton;

        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private Image _bestTimesScreen;

        [SerializeField]
        private SceneLoader _sceneLoader;

        private void Awake()
        {
            Ensure.NotNull(_playButton);
            Ensure.NotNull(_bestTimesButton);
            Ensure.NotNull(_exitButton);
            Ensure.NotNull(_sceneLoader);

            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _bestTimesButton.onClick.AddListener(OnBestTimesButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            ULog.Trace("");
            _sceneLoader.LoadGameScene("Game");
        }

        private void OnBestTimesButtonClicked()
        {
            ULog.Trace("");
            _bestTimesScreen.gameObject.SetActive(true);
        }

        private void OnExitButtonClicked()
        {
            ULog.Trace("");
            Application.Quit();
        }
    }
}
