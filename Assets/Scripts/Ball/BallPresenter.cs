using R3;
using R3.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public sealed class BallPresenter : MonoBehaviour
    {
        private const string PaddleColliderName = "PaddleGraphic";

        [SerializeField]
        [Range(0f, 100f)]
        private float _initialForce = 50f;
        [SerializeField]
        [Range(0f, 90f)]
        private float _initialAngle = 45f;
        [SerializeField]
        [Tooltip("How much damage I can inflict on a brick per collision.")]
        private int _power = 1;
        [SerializeField]
        [Range(0f, 90f)]
        [Tooltip("0: completely vertical, 90: +/- 90degrees from the up vector")]
        private float _maxPaddleBounceAngle = 75f;

        private Rigidbody2D _rigidbody;
        private float _maxPaddleBounceAngleInRadians;

        public void Init(Ball ball = null)
        {
            Ball = ball ?? new Ball(_initialForce, _power, Vector3.zero);
            _rigidbody = GetComponent<Rigidbody2D>();
            _maxPaddleBounceAngleInRadians = _maxPaddleBounceAngle * Mathf.Deg2Rad;

            this
                .OnTriggerEnter2DAsObservable()
                .Where(collider => collider.CompareTag(Tags.DeadZone))
                .Subscribe(_ => Ball.Active.Value = false)
                .AddTo(this);

            this
                .OnCollisionEnter2DAsObservable()
                .Where(collision => collision.gameObject.name == PaddleColliderName)
                .Subscribe(CalculateBounceVelocity)
                .AddTo(this);

            Ball
                .Active
                .Subscribe(value => gameObject.SetActive(value))
                .AddTo(this);
        }

        public Ball Ball { get; set; }

        public Vector2 Velocity => _rigidbody.linearVelocity;

        public void AddInitialForce()
        {
            transform.parent = null;

            var force = new Vector2
            {
                x = Mathf.Sin(_initialAngle * Mathf.Deg2Rad) * Ball.InitialForce,
                y = Mathf.Cos(_initialAngle * Mathf.Deg2Rad) * Ball.InitialForce
            };

            _rigidbody.AddForce(force);
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

            // Map the horizontal contact point to the (0, 1) range.
            // Input is in the range (-paddleWidth/2, paddleWidth/2)
            var normalizedLocalContactX = localContact.x / paddleWidth + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + _maxPaddleBounceAngleInRadians,
                Mathf.PI / 2 - _maxPaddleBounceAngleInRadians,
                normalizedLocalContactX
            );

            var bounceForce = new Vector2
            {
                x = Mathf.Cos(bounceAngle) * Ball.InitialForce,
                y = Mathf.Sin(bounceAngle) * Ball.InitialForce
            };

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(bounceForce);
        }
    }
}
