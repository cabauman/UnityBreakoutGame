using GameCtor.FuseDI;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace BreakoutGame
{
    public sealed partial class Hud : MonoBehaviour
    {
        [SerializeField]
        private LifeTracker _lifeTracker;

        // TODO: Consider replacing this with a GameState or similar
        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private TextMeshProUGUI _numLivesLabel;

        [SerializeField]
        private TextMeshProUGUI _levelLabel;

        [SerializeField]
        private TextMeshProUGUI _bestTimeLabel;

        [SerializeField]
        private LocalizedString _livesString;

        [Inject]
        private LevelEvents _levelEvents;

        void OnEnable()
        {
            _livesString.Arguments = new object[] { 0 };
            _livesString.StringChanged += UpdateString;
        }

        void OnDisable()
        {
            _livesString.StringChanged -= UpdateString;
        }

        private void Start()
        {
            _lifeTracker
                .NumLives
                .Subscribe(UpdateLives)
                .AddTo(this);

            _gameManager
                .Level
                .Subscribe(level => _levelLabel.text = $"Level: {level}")
                .AddTo(this);

            _levelEvents
                .LevelStarted
                .Subscribe(level => UpdateBestTime(level))
                .AddTo(this);
        }

        private void UpdateLives(int lives)
        {
            _livesString.Arguments[0] = lives;
            _livesString.RefreshString();
        }

        private void UpdateString(string localizedString)
        {
            _numLivesLabel.text = localizedString;
        }

        private void UpdateBestTime(int level)
        {
            var bestTimeKey = $"BestTime_Level_{level}";
            if (PlayerPrefs.HasKey(bestTimeKey))
            {
                var bestTime = PlayerPrefs.GetFloat(bestTimeKey);
                _bestTimeLabel.text = $"Best Time: {bestTime:F2}";
            }
            else
            {
                _bestTimeLabel.text = "Best Time: N/A";
            }
        }
    }
}
