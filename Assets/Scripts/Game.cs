using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class Game
{
    private Ball _ball;
    private int _defaultNumLives;
    private bool _gameOver = false;

    public Game(Paddle paddle, IList<Brick> bricks, int numLives)
    {
        Paddle = paddle;
        Bricks = bricks;
        _defaultNumLives = numLives;
        NumLives = new ReactiveProperty<int>(numLives);
        BricksRemaining = new ReactiveProperty<int>(Bricks.Count);

        Bricks
            .ToObservable()
            .SelectMany(x => x.Active)
            .Where(active => active == false)
            .Subscribe(_ => BricksRemaining.Value -= 1);

        GameWon = BricksRemaining
            .Where(count => count == 0)
            .Do(x => _ball.Active.Value = false)
            .Select(_ => Unit.Default);

        var noBallsInPlay = NumBallsInPlay
            .Where(count => count == 0);

        noBallsInPlay
            .Where(_ => NumLives.Value > 0)
            .Subscribe(_ => UseExtraLife());

        GameLost = noBallsInPlay
            .Where(_ => NumLives.Value == 0)
            .Select(_ => Unit.Default);

        ResetGameCmd = new ReactiveCommand();
        ResetGameCmd.Subscribe(_ => ResetGame());
    }

    public Paddle Paddle { get; }

    public IList<Brick> Bricks { get; }

    public IObservable<Unit> GameWon { get; }

    public IObservable<Unit> GameLost { get; }

    public IReactiveProperty<int> NumBallsInPlay { get; }

    public IReactiveProperty<int> BricksRemaining { get; }

    public IReactiveProperty<int> NumLives { get; }

    public ReactiveCommand ResetGameCmd { get; }

    private void ResetGame()
    {
        NumLives.Value = _defaultNumLives;
        Paddle.ResetBallPos.Execute(Unit.Default);
        _ball.Active.Value = true;
        BricksRemaining.Value = Bricks.Count;

        foreach (var brick in Bricks)
        {
            brick.ResetHp.Execute(Unit.Default);
        }

        _gameOver = false;
    }

    private void UseExtraLife()
    {
        Paddle.ResetBallPos.Execute(Unit.Default);
        _ball.Active.Value = true;
        NumBallsInPlay.Value += 1;
        NumLives.Value -= 1;
    }

    private IObservable<Ball> DetectWhenBallHitsDeadZone(Ball ball)
    {
        return ball.Active
            .Where(x => x == false)
            .Select(_ => ball)
            .Take(1);
    }
}
