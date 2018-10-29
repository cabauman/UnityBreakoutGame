using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    private GamePresenter _gamePresenter;
    [SerializeField]
    private TextMeshProUGUI _gameWonLabel;
    [SerializeField]
    private TextMeshProUGUI _gameLostLabel;
    [SerializeField]
    private Button _playAgainButton;

    [Header("Parameters")]
    [SerializeField]
    private float _delayBeforeDisplayingPlayAgainButton = 3f;

    private void Start()
    {
        _gamePresenter.Game
            .GameWon
            .Subscribe(_ => _gameWonLabel.gameObject.SetActive(true))
            .AddTo(this);

        _gamePresenter.Game
            .GameLost
            .Subscribe(_ => _gameLostLabel.gameObject.SetActive(true))
            .AddTo(this);

        Observable
            .Merge(_gamePresenter.Game.GameWon, _gamePresenter.Game.GameLost)
            .Delay(TimeSpan.FromSeconds(_delayBeforeDisplayingPlayAgainButton))
            .Subscribe(_ => _playAgainButton.gameObject.SetActive(true))
            .AddTo(this);

        _gamePresenter.Game.ResetGameCmd.BindToOnClick(_playAgainButton, _ => HideUI());
    }

    private void HideUI()
    {
        _gameWonLabel.gameObject.SetActive(false);
        _gameLostLabel.gameObject.SetActive(false);
        _playAgainButton.gameObject.SetActive(false);
    }
}
