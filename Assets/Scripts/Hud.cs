using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TextMeshProUGUI _gameWonLabel;
    [SerializeField]
    private TextMeshProUGUI _gameLostLabel;
    [SerializeField]
    private Button _playAgainButton;

    [SerializeField]
    private float _delayBeforeDisplayingPlayAgainButton = 3f;

    private void Start()
    {
        _gameManager
            .GameWon
            .Subscribe(_ => _gameWonLabel.gameObject.SetActive(true))
            .AddTo(this);

        _gameManager
            .GameLost
            .Subscribe(
                _ =>
                {
                    _gameLostLabel.gameObject.SetActive(true);
                })
            .AddTo(this);

        Observable
            .Merge(_gameManager.GameWon, _gameManager.GameLost)
            .Delay(TimeSpan.FromSeconds(_delayBeforeDisplayingPlayAgainButton))
            .Subscribe(_ => _playAgainButton.gameObject.SetActive(true))
            .AddTo(this);

        _gameManager.ResetGameCmd.BindToOnClick(_playAgainButton, _ => HideUI());
    }

    private void HideUI()
    {
        _gameWonLabel.gameObject.SetActive(false);
        _gameLostLabel.gameObject.SetActive(false);
        _playAgainButton.gameObject.SetActive(false);
    }
}
