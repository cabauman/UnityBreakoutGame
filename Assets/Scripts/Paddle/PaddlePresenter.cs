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

        private IBallPaddleCollisionStrategy _defaultCollisionStrategy;
        private IGameWorldEffect _gameWorldEffect; 
        private List<BallPresenter> _attachedBalls = new();

        public PaddlePresenter(GameObject view, IBallPaddleCollisionStrategy collisionStrategy, Paddle.Config config)
        {
            _view = view;
            _collisionStrategy = collisionStrategy;
            _defaultCollisionStrategy = collisionStrategy;
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

        // public void Tick(float deltaTime)
        // {
        // }

        public void SetBallCollisionStrategy(IBallPaddleCollisionStrategy strategy)
        {
            Assert.IsNotNull(strategy);
            _collisionStrategy = strategy;
            _gameWorldEffect?.Disable();
            _gameWorldEffect = null;
        }

        public void SetGameWorldEffect(IGameWorldEffect effect)
        {
            Assert.IsNotNull(effect);
            _collisionStrategy = _defaultCollisionStrategy;
            _gameWorldEffect?.Disable();
            _gameWorldEffect = effect;
            _gameWorldEffect.Enable();
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
            ball.AttachTo(Trfm);
            _attachedBalls.Add(ball);
        }

        private void LaunchBalls()
        {
            foreach (var ball in _attachedBalls)
            {
                ball.Trfm.parent = null;

                var force = PlaneBouncingUtility.CalculateBounceDirection(
                    GraphicTrfm.position,
                    Vector2.up,
                    Width,
                    _config._ballObj.transform.position,
                    _config.MaxBounceAngleRad,
                    _config._ballLaunchForce
                );

                ball.SetForce(force);
            }
            _attachedBalls.Clear();
        }

        private void ResetBallPos_()
        {
            _collisionStrategy = _defaultCollisionStrategy;
            _gameWorldEffect?.Disable();

            _config._ballObj.Presenter.Active.Value = true;
            AttachBall(_config._ballObj.Presenter);
            _config._ballObj.transform.position = _config._initialBallPosTrfm.position;
        }
    }

    public interface IGameWorldEffect
    {
        void Enable();
        void Disable();
    }
}