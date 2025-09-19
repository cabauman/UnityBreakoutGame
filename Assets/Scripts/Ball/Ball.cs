using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Ball
    {
        private const string PADDLE_COLLIDER_NAME = "PaddleGraphic";

        private readonly GameObject _view;
        private readonly BallPresenter.Config _config;
        private readonly Rigidbody2D _rigidbody;
        private readonly float _maxPaddleBounceAngleRad;

        public Ball(GameObject view, BallPresenter.Config config)
        {
            _view = view;
            _config = config;
            InitialForce = config._initialForce;
            Power = config._power;
            StartPosition = Vector3.zero;
            Active = new ReactiveProperty<bool>(true);

            _rigidbody = view.GetComponent<Rigidbody2D>();
            _maxPaddleBounceAngleRad = config._maxPaddleBounceAngle * Mathf.Deg2Rad;

            _view
                .OnTriggerEnter2DAsObservable()
                .Where(collider => collider.CompareTag(Tags.DEAD_ZONE))
                .Subscribe(_ => this.Active.Value = false)
                .AddTo(_view);

            _view
                .OnCollisionEnter2DAsObservable()
                .Where(collision => collision.gameObject.name == PADDLE_COLLIDER_NAME)
                .Subscribe(CalculateBounceVelocity)
                .AddTo(_view);

            this
                .Active
                .Subscribe(value => view.SetActive(value))
                .AddTo(view);
        }

        public float InitialForce { get; }

        public int Power { get; }

        public Vector3 StartPosition { get; }

        public IReactiveProperty<bool> Active { get; }

        public void AddInitialForce()
        {
            _view.transform.parent = null;
            var force = GetInitialForce(_config._initialAngle);
            _rigidbody.AddForce(force);
        }

        public Vector2 GetInitialForce(float angleDeg)
        {
            return new Vector2
            {
                x = Mathf.Sin(angleDeg * Mathf.Deg2Rad) * InitialForce,
                y = Mathf.Cos(angleDeg * Mathf.Deg2Rad) * InitialForce
            };
        }

        /// <summary>
        /// Allows the player to control bounce angle regardless of the incoming ball angle.
        /// Contact on the left side of the paddle makes the ball go left.
        /// Contact on the right side of the paddle makes the ball go right.
        /// Angle is scaled based on how close to the paddle edge it hits.
        /// </summary>
        private void CalculateBounceVelocity(Collision2D collision)
        {
            var localContact = collision.transform.InverseTransformPoint(collision.contacts[0].point);
            var paddleWidth = collision.collider.GetComponent<SpriteRenderer>().bounds.size.x;

            var bounceForce = CalculatePaddleBounceForce(localContact.x, paddleWidth, _maxPaddleBounceAngleRad);

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(bounceForce);
        }

        public Vector2 CalculatePaddleBounceForce(float localContactX, float paddleWidth, float maxBounceAngleRad)
        {
            // Map the horizontal contact point to the (0, 1) range.
            // Input is in the range (-paddleWidth/2, paddleWidth/2)
            var normalizedLocalContactX = localContactX / paddleWidth + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + maxBounceAngleRad,
                Mathf.PI / 2 - maxBounceAngleRad,
                normalizedLocalContactX
            );

            return new Vector2
            {
                x = Mathf.Cos(bounceAngle) * InitialForce,
                y = Mathf.Sin(bounceAngle) * InitialForce
            };
        }
    }
}