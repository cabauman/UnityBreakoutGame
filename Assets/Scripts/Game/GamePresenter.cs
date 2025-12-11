using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public sealed class GamePresenter : MonoBehaviour
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
                .Where(_ => Mouse.current.leftButton.wasPressedThisFrame &&
                    EventSystem.current != null &&
                    !EventSystem.current.IsPointerOverGameObject() &&
                    Mathf.Abs(_ballPresenter.Velocity.y) < Mathf.Epsilon)
                .Subscribe(_ => _ballPresenter.AddInitialForce())
                .AddTo(this);
        }

        public Game Game { get; private set; }

        private void InstantiateBonusBall(Ball ball)
        {
            var ballPresenter = Instantiate(_ballPresenterPrefab, ball.StartPosition, Quaternion.identity);
            ballPresenter.Init(ball);
            ballPresenter.AddInitialForce();
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
