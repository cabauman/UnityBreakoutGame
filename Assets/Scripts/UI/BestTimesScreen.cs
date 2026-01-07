using GameCtor.DevToolbox;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutGame
{
    public sealed class BestTimesScreen : MonoBehaviour
    {
        [Header("Object References")]

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private Image _screen;

        private void Awake()
        {
            Ensure.NotNull(_backButton);
            Ensure.NotNull(_screen);

            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            ULog.Trace("");
            _screen.gameObject.SetActive(false);
        }
    }
}
