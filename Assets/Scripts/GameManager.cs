using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    private BallPresenter _ballPresenter;
    [SerializeField]
    private PaddlePresenter _paddlePresenter;
    [SerializeField]
    private List<BrickPresenter> _brickPresenters;

    private void Awake()
    {
        BricksRemaining = new ReactiveProperty<int>(_brickPresenters.Count);

        _brickPresenters
            .ToObservable()
            .SelectMany(x => x.Brick.Active.AsObservable())
            .Where(active => active == false)
            .Subscribe(_ => BricksRemaining.Value -= 1)
            .AddTo(this);

        GameWon = BricksRemaining
            .Where(count => count == 0)
            .Select(_ => Unit.Default);

        GameLost = _ballPresenter
            .Ball
            .Active
            .Where(active => active == false)
            .Select(_ => Unit.Default);

        ResetGameCmd = new ReactiveCommand();
        ResetGameCmd.Subscribe(_ => ResetGame());
    }

    public IObservable<Unit> GameWon { get; private set; }

    public IObservable<Unit> GameLost { get; private set; }

    public IReactiveProperty<int> BricksRemaining { get; private set; }

    public ReactiveCommand ResetGameCmd { get; private set; }

    private void ResetGame()
    {
        _paddlePresenter.Paddle.ResetBallPos.Execute(Unit.Default);
        _ballPresenter.Ball.Active.Value = true;
        BricksRemaining.Value = _brickPresenters.Count;

        foreach (var brickPresenter in _brickPresenters)
        {
            brickPresenter.Brick.ResetHp.Execute(Unit.Default);
        }
    }
}
