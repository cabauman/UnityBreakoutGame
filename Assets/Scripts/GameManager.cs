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

    private bool _gameOver = false;
    private int _defaultNumLives;

    private void Awake()
    {
        BricksRemaining = new ReactiveProperty<int>(_brickPresenters.Count);
        _defaultNumLives = 3;
        NumLives = new ReactiveProperty<int>(3);
        NumBallsInPlay = new ReactiveProperty<int>(1);

        _ballPresenter.Ball.Active
            .Where(active => !_gameOver && !active)
            .Subscribe(
                _ =>
                {
                    NumBallsInPlay.Value -= 1;
                })
            .AddTo(this);

        _brickPresenters
            .ToObservable()
            .SelectMany(x => x.Brick.Active)
            .Where(active => active == false)
            .Subscribe(_ => BricksRemaining.Value -= 1)
            .AddTo(this);

        GameWon = BricksRemaining
            .Where(count => count == 0)
            .Do(
                x =>
                {
                    _gameOver = true;
                    _ballPresenter.Ball.Active.Value = false;
                })
            .Select(_ => Unit.Default);

        //BallPresenters = new ReactiveCollection<BallPresenter>();
        //BallPresenters
        //    .ObserveAdd()
        //    .Select(addEvent => addEvent.Value)
        //    .SelectMany(ballPresenter => DetectWhenBallHitsDeadZone(ballPresenter))
        //    .Subscribe(ballPresenter => BallPresenters.Remove(ballPresenter));

        //BallPresenters.Add(_ballPresenter);

        var noBallsInPlay = NumBallsInPlay
            .Where(count => !_gameOver && count == 0)
            .Publish()
            .RefCount();

        noBallsInPlay
            .Where(_ => NumLives.Value > 0)
            .Delay(TimeSpan.FromMilliseconds(100))
            .Subscribe(_ => UseExtraLife());

        GameLost = noBallsInPlay
            .Where(_ => NumLives.Value == 0)
            .Select(_ => Unit.Default);

        ResetGameCmd = new ReactiveCommand();
        ResetGameCmd.Subscribe(_ => ResetGame());
    }

    public IObservable<Unit> GameWon { get; private set; }

    public IObservable<Unit> GameLost { get; private set; }

    //public IReactiveCollection<BallPresenter> BallPresenters { get; private set; }

    public IReactiveProperty<int> BricksRemaining { get; private set; }

    public IReactiveProperty<int> NumBallsInPlay { get; private set; }

    public IReactiveProperty<int> NumLives { get; private set; }

    public ReactiveCommand ResetGameCmd { get; private set; }

    private void ResetGame()
    {
        NumLives.Value = _defaultNumLives;
        _paddlePresenter.Paddle.ResetBallPos.Execute(Unit.Default);
        _ballPresenter.Ball.Active.Value = true;
        //BallPresenters.Clear();
        //BallPresenters.Add(_ballPresenter);
        NumBallsInPlay.Value = 1;
        BricksRemaining.Value = _brickPresenters.Count;

        foreach (var brickPresenter in _brickPresenters)
        {
            brickPresenter.Brick.ResetHp.Execute(Unit.Default);
        }

        _gameOver = false;
    }

    private void UseExtraLife()
    {
        _paddlePresenter.Paddle.ResetBallPos.Execute(Unit.Default);
        _ballPresenter.Ball.Active.Value = true;
        //BallPresenters.Add(_ballPresenter);
        NumLives.Value -= 1;
        NumBallsInPlay.Value = 1;
    }

    private IObservable<BallPresenter> DetectWhenBallHitsDeadZone(BallPresenter ballPresenter)
    {
        return ballPresenter.Ball.Active
            .Where(x => x == false)
            .Select(_ => ballPresenter)
            .Take(1);
    }
}
