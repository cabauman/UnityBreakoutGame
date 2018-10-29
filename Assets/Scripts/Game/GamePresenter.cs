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

    [Header("Parameters")]
    [SerializeField]
    private uint _defaultNumLives = 1;

    private void Awake()
    {
        _ballPresenter.Init();
        _paddlePresenter.Init();
        _brickPresenters.ForEach(x => x.Init());

        var ball = _ballPresenter.Ball;
        var paddle = _paddlePresenter.Paddle;
        var bricks = _brickPresenters.Select(x => x.Brick).ToList();
        Game = new Game(ball, paddle, bricks, _defaultNumLives);
    }

    private void InstantiateBall(Ball ball)
    {
        var ballPresenter = Instantiate<BallPresenter>(null, transform.position, Quaternion.identity);
        ballPresenter.Ball = ball;
    }

    public Game Game { get; private set; }
}
