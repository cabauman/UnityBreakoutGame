using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class Paddle
    {
        private IBallPaddleCollisionStrategy _collisionStrategy;

        private float _screenWidth;
        private PaddlePresenter _view;
        private PaddlePresenter.Config _config;

        public Paddle(PaddlePresenter view, PaddlePresenter.Config config)
        {
            _view = view;
            _config = config;
            _screenWidth = Screen.width;
            Width = new ReactiveProperty<float>(1);
            ResetBallPos = new ReactiveCommand<Unit>();

            this
                .Width
                .Subscribe(xScale => _config._graphicTrfm.localScale = new Vector3(xScale, _config._graphicTrfm.localScale.y))
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

        public IReactiveProperty<float> Width { get; }

        public ReactiveCommand<Unit> ResetBallPos { get; }

        public void SetCollisionStrategy(IBallPaddleCollisionStrategy strategy)
        {
            _collisionStrategy = strategy;
        }

        public void OnBallCollision(Ball ball)
        {
            _collisionStrategy.HandleCollision(ball, this);
        }

        private void UpdateXPosition(Vector3 mousePos)
        {
            mousePos.x = Mathf.Clamp(mousePos.x, 0, _screenWidth);
            var xPos = Camera.main.ScreenToWorldPoint(mousePos).x;
            _view.transform.position = new Vector3(xPos, _view.transform.position.y, _view.transform.position.z);
        }

        private void ResetBallPos_()
        {
            _config._ballPresenter.transform.parent = _view.transform;
            _config._ballPresenter.transform.position = _config._initialBallPosTrfm.position;
        }
    }
}