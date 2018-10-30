using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class BallPresenter : MonoBehaviour
{
    [SerializeField]
    private float _initialForce = 50f;
    [SerializeField]
    private int _power = 1;
    [SerializeField]
    [Range(0f, 90f)]
    private float _maxPaddleBounceAngle = 75f;

    private Rigidbody2D _rigidBody;
    private float _maxPaddleBounceAngleInRadians;

    public void Init(Ball ball = null)
    {
        Ball = ball ?? new Ball(_initialForce, _power, Vector3.zero);
        _rigidBody = GetComponent<Rigidbody2D>();
        _maxPaddleBounceAngleInRadians = _maxPaddleBounceAngle * Mathf.Deg2Rad;

        Observable
            .EveryUpdate()
            .Where(_ => Input.GetButtonDown("Fire1"))
            .Subscribe(_ => PutBallIntoPlay())
            .AddTo(this);

        this
            .OnTriggerEnter2DAsObservable()
            .Where(collider => collider.tag == Tags.DEAD_ZONE)
            .Subscribe(_ => DeactivateBall())
            .AddTo(this);

        this
            .OnCollisionEnter2DAsObservable()
            .Where(collision => collision.gameObject.name.StartsWith("Graphic"))
            .Subscribe(CalculateBounceVelocity)
            .AddTo(this);

        Ball
            .Active
            .Subscribe(value => gameObject.SetActive(value))
            .AddTo(this);
    }

    public Ball Ball { get; set; }

    public void PutBallIntoPlay()
    {
        transform.parent = null;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Ball.InitialForce * Mathf.Sin(45f * Mathf.Deg2Rad), Ball.InitialForce * Mathf.Cos(45f * Mathf.Deg2Rad)));
    }

    private void DeactivateBall()
    {
        Ball.Active.Value = false;
        transform.position = new Vector3(-100f, -100f);
    }

    private void CalculateBounceVelocity(Collision2D collision)
    {
        var localContact = collision.transform.InverseTransformPoint(collision.contacts[0].point);
        var paddleWidth = collision.collider.GetComponent<SpriteRenderer>().bounds.size.x;
        var normalizedLocalContactX = localContact.x / (paddleWidth / 2);
        var bounceAngle = normalizedLocalContactX * _maxPaddleBounceAngleInRadians;

        var bounceForce = new Vector2
        {
            x = Mathf.Sin(bounceAngle) * Ball.InitialForce,
            y = Mathf.Cos(bounceAngle) * Ball.InitialForce
        };

        _rigidBody.velocity = Vector2.zero;
        _rigidBody.AddForce(bounceForce);
    }
}
