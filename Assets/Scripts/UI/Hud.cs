using TMPro;
using R3;
using UnityEngine;
using UnityEngine.Localization;

namespace BreakoutGame
{
    public sealed class Hud : MonoBehaviour
    {
        [SerializeField]
        private GamePresenter _gamePresenter;
        [SerializeField]
        private TextMeshProUGUI _numLivesLabel;
        [SerializeField]
        private LocalizedString livesString;

        void OnEnable()
        {
            livesString.Arguments = new object[] { 0 };
            livesString.StringChanged += UpdateString;
        }

        void OnDisable()
        {
            livesString.StringChanged -= UpdateString;
        }

        private void Start()
        {
            _gamePresenter.Game
                .NumLives
                .Subscribe(UpdateLives)
                .AddTo(this);
        }

        private void UpdateLives(uint lives)
        {
            livesString.Arguments[0] = lives;
            livesString.RefreshString();
        }

        private void UpdateString(string localizedString)
        {
            _numLivesLabel.text = localizedString;
        }
    }
}
