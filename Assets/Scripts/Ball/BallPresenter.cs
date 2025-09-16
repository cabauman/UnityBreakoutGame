using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public class BallPresenter : MonoBehaviour
    {
        private const string PADDLE_COLLIDER_NAME = "PaddleGraphic";

        [SerializeField]
        [Range(0f, 100f)]
        private float _initialForce = 50f;
        [SerializeField]
        [Range(-90f, 90f)]
        private float _initialAngle = 45f;
        [SerializeField]
        [Tooltip("How much damage I can inflict on a brick per collision.")]
        private int _power = 1;
        [SerializeField]
        [Range(0f, 90f)]
        [Tooltip("0: completely vertical, 90: +/- 90degrees from the up vector")]
        private float _maxPaddleBounceAngle = 75f;

        private Rigidbody2D _rigidbody;
        private float _maxPaddleBounceAngleRad;

        public void Init(Ball ball = null)
        {
            Ball = ball ?? new Ball(_initialForce, _power, Vector3.zero);
            _rigidbody = GetComponent<Rigidbody2D>();
            _maxPaddleBounceAngleRad = _maxPaddleBounceAngle * Mathf.Deg2Rad;

            this
                .OnTriggerEnter2DAsObservable()
                .Where(collider => collider.CompareTag(Tags.DEAD_ZONE))
                .Subscribe(_ => Ball.Active.Value = false)
                .AddTo(this);

            this
                .OnCollisionEnter2DAsObservable()
                .Where(collision => collision.gameObject.name == PADDLE_COLLIDER_NAME)
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
            var force = Ball.GetInitialForce(_initialAngle);
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

            var bounceForce = Ball.CalculatePaddleBounceForce(localContact.x, paddleWidth, _maxPaddleBounceAngleRad);

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(bounceForce);
        }
    }
}