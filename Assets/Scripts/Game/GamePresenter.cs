using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class GamePresenter : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField]
        private BallPresenter _ballPresenter;
        [SerializeField]
        private PaddlePresenter _paddlePresenter;
        [SerializeField]
        private List<BrickPresenter> _brickPresenters;
        [SerializeField]
        private BallPresenter _ballPresenterPrefab;

        [Header("Parameters")]
        [SerializeField]
        private uint _defaultNumLives = 1;

        private readonly List<GameObject> _bonusBalls = new();

        private void Awake()
        {
            _ballPresenter.Init();
            _paddlePresenter.Init();
            _brickPresenters.ForEach(x => x.Init());

            var ball = _ballPresenter.Ball;
            var paddle = _paddlePresenter.Paddle;
            var bricks = _brickPresenters.Select(x => x.Brick).ToList();
            Game = new Game(ball, paddle, bricks, _defaultNumLives);

            Game
                .CreateBonusBall
                .Subscribe(InstantiateBonusBall)
                .AddTo(this);

            Game
                .GameWon
                .Subscribe(_ => ClearBonusBalls())
                .AddTo(this);

            Observable
                .EveryUpdate()
                .Where(_ =>
                    UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
                .Subscribe(_ => _ballPresenter.Ball.AddInitialForce())
                .AddTo(this);
        }

        public Game Game { get; private set; }

        private void InstantiateBonusBall(Vector3 spawnPosition)
        {
            var ballPresenter = Instantiate(_ballPresenterPrefab, spawnPosition, Quaternion.identity);
            ballPresenter.Init();
            ballPresenter.Ball.AddInitialForce();
            _bonusBalls.Add(ballPresenter.gameObject);
        }

        private void ClearBonusBalls()
        {
            foreach (var ball in _bonusBalls)
            {
                Destroy(ball);
            }

            _bonusBalls.Clear();
        }
    }
}