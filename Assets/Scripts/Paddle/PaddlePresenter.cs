using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace BreakoutGame
{
    public sealed class PaddlePresenter
    {
        private readonly GameObject _view;
        private IBallPaddleCollisionStrategy _collisionStrategy;
        private readonly Paddle.Config _config;

        private List<BallPresenter> _attachedBalls = new();

        public PaddlePresenter(GameObject view, IBallPaddleCollisionStrategy collisionStrategy, Paddle.Config config)
        {
            _view = view;
            _collisionStrategy = collisionStrategy;
            _config = config;
            WidthScale = new ReactiveProperty<float>(1f);
            ResetBallPos = new ReactiveCommand<Unit>();

            this
                .WidthScale
                .Subscribe(xScale => GraphicTrfm.localScale = new Vector3(xScale, GraphicTrfm.localScale.y, 1f));

            this
                .ResetBallPos
                .Subscribe(_ => ResetBallPos_());

            Observable
                .EveryUpdate()
                .Where(_ => UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
                .Subscribe(_ => LaunchBalls())
                .AddTo(_view);

            AttachBall(_config._ballObj.Presenter);

            //_config._gameplayStartedEvent.OnEventRaised += ResetBallPos_;
        }

        public IReactiveProperty<float> WidthScale { get; }

        public float Width => _config._spriteRenderer.bounds.size.x * WidthScale.Value;

        public ReactiveCommand<Unit> ResetBallPos { get; }

        public Transform GraphicTrfm => _config._spriteRenderer.transform;

        public Transform Trfm => _view.transform;

        public void Tick(float deltaTime)
        {
            //var mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            //UpdateXPosition(new Vector3(mousePos.x, mousePos.y, 10f));
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

        public void AttachBall(BallPresenter ball)
        {
            Assert.IsFalse(_attachedBalls.Contains(ball));
            _attachedBalls.Add(ball);
            ball.Trfm.parent = _view.transform;
        }

        private static Vector2 CalculateBallLaunchForce(BallPresenter ball, PaddlePresenter paddle, Vector2 point)
        {
            var localContact = paddle.GraphicTrfm.InverseTransformPoint(point);

            // Map the horizontal contact point to the (0, 1) range.
            // Input is in the range (-paddleWidth/2, paddleWidth/2)
            var normalizedLocalContactX = localContact.x / paddle.Width + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + paddle._config.MaxBounceAngleRad,
                Mathf.PI / 2 - paddle._config.MaxBounceAngleRad,
                normalizedLocalContactX
            );

            var bounceForce = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle)) * paddle._config._ballLaunchForce;
            return bounceForce;
        }

        private void LaunchBalls()
        {
            foreach (var ball in _attachedBalls)
            {
                ball.Trfm.parent = null;
                var force = CalculateBallLaunchForce(ball, this, ball.Trfm.position);
                ball.SetForce(force);
            }
            _attachedBalls.Clear();
        }

        private void UpdateXPosition(Vector3 mousePos)
        {
            var paddleHalfWidth = Width / 2f;

            float halfScreenWidthWorld = Camera.main.orthographicSize * Camera.main.aspect;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            float clampedX = Mathf.Clamp(mouseWorldPos.x, -halfScreenWidthWorld + paddleHalfWidth, halfScreenWidthWorld - paddleHalfWidth);


            _view.transform.position = new Vector3(clampedX, _view.transform.position.y, _view.transform.position.z);
        }

        private void ResetBallPos_()
        {
            _config._ballObj.Presenter.Active.Value = true;
            AttachBall(_config._ballObj.Presenter);
            _config._ballObj.transform.position = _config._initialBallPosTrfm.position;
        }
    }
}