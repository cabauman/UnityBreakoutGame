using TMPro;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Hud : MonoBehaviour
    {
        private const string NUM_LIVES_FMT = "Lives: {0}";

        [SerializeField]
        private GamePresenter _gamePresenter;
        [SerializeField]
        private TextMeshProUGUI _numLivesLabel;

        private void Start()
        {
            _gamePresenter.Game
                .NumLives
                .Subscribe(numLives => _numLivesLabel.text = string.Format(NUM_LIVES_FMT, numLives))
                .AddTo(this);
        }
    }
}
