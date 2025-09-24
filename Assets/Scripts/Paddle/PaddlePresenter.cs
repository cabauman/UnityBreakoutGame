using System;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddlePresenter
    {
        private IBallPaddleCollisionStrategy _collisionStrategy;

        private readonly float _screenWidth;
        private readonly GameObject _view;
        private readonly Paddle.Config _config;

        public IReactiveProperty<float> WidthScale { get; }

        public float Width => _config._spriteRenderer.bounds.size.x * WidthScale.Value;

        public ReactiveCommand<Unit> ResetBallPos { get; }

        public Transform GraphicTrfm => _config._spriteRenderer.transform;

        public PaddlePresenter(GameObject view, Paddle.Config config)
        {
            _view = view;
            _config = config;
            _screenWidth = Screen.width;
            WidthScale = new ReactiveProperty<float>(1);
            ResetBallPos = new ReactiveCommand<Unit>();

            this
                .WidthScale
                .Subscribe(xScale => GraphicTrfm.localScale = new Vector3(xScale, GraphicTrfm.localScale.y))
                .AddTo(_view);

            this
                .ResetBallPos
                .Subscribe(_ => ResetBallPos_())
                .AddTo(_view);
        }

        public void Tick(float deltaTime)
        {
            var mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            UpdateXPosition(new Vector3(mousePos.x, mousePos.y, 10f));
        }

        public void SetCollisionStrategy(IBallPaddleCollisionStrategy strategy)
        {
            _collisionStrategy = strategy;
        }

        public void OnCollisionEnter2D(GameObject other, Vector2 point)
        {
            if (other.TryGetComponent<Ball>(out var ball))
            {
                _collisionStrategy.HandleCollision(ball.Presenter, this, point);
            }
        }

        //public void OnBallCollision(BallPresenter ball, Vector2 point)
        //{
        //    _collisionStrategy.HandleCollision(ball, this);
        //}

        private void UpdateXPosition(Vector3 mousePos)
        {
            mousePos.x = Mathf.Clamp(mousePos.x, 0, _screenWidth);
            var xPos = Camera.main.ScreenToWorldPoint(mousePos).x;
            _view.transform.position = new Vector3(xPos, _view.transform.position.y, _view.transform.position.z);
        }

        private void ResetBallPos_()
        {
            _config._ballObj.transform.parent = _view.transform;
            _config._ballObj.transform.position = _config._initialBallPosTrfm.position;
        }
    }
}