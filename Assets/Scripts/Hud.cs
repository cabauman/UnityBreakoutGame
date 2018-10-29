using TMPro;
using UniRx;
using UnityEngine;

public class Hud : MonoBehaviour
{
    private const string NUM_LIVES_FMT = "Lives: {0}";

    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TextMeshProUGUI _numLivesLabel;

    private void Start()
    {
        _gameManager
            .NumLives
            .Subscribe(numLives => _numLivesLabel.text = string.Format(NUM_LIVES_FMT, numLives))
            .AddTo(this);
    }
}
