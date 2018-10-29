using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    private BallPresenter _ballPresenter;
    [SerializeField]
    private PaddlePresenter _paddlePresenter;
    [SerializeField]
    private List<BrickPresenter> _brickPresenters;

    private int _numLives = 1;

    private Game _game;

    private void Awake()
    {
        var paddle = _paddlePresenter.Paddle;
        var bricks = _brickPresenters.Select(x => x.Brick).ToList();
        _game = new Game(paddle, bricks, _numLives);

        //_game.BallsInPlay
        //    .ObserveAdd()
        //    .Select(addEvent => addEvent.Value)
        //    .Subscribe(InstantiateBall)
        //    .AddTo(this);

        //_game.BallsInPlay
        //    .ObserveRemove()
        //    .Select(removeEvent => removeEvent.Index)
        //    .Subscribe(index => BallPresenters.RemoveAt(index))
        //    .AddTo(this);
    }

    public IList<BallPresenter> BallPresenters { get; private set; }

    private void ResetGame()
    {
    }

    private void InstantiateBall(Ball ball)
    {
        var ballPresenter = Instantiate<BallPresenter>(null, transform.position, Quaternion.identity);
        ballPresenter.Ball = ball;
        BallPresenters.Add(ballPresenter);
    }
}
