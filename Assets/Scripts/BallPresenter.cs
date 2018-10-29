using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class BallPresenter : MonoBehaviour
{
    private const string TAG_DEAD_ZONE = "DeadZone";

    [SerializeField]
    private float _initialForce = 50f;
    [SerializeField]
    private int _power = 1;

    private void Start()
    {
        //Ball = new Ball(_initialForce, _power);

        Observable
            .EveryUpdate()
            .Where(_ => Input.GetButtonDown("Fire1")/* && ballInPlay*/)
            .Subscribe(_ => PutBallIntoPlay())
            .AddTo(this);

        this
            .OnTriggerEnter2DAsObservable()
            .Where(collider => collider.tag == TAG_DEAD_ZONE)
            .Subscribe(_ => DeactivateBall())
            .AddTo(this);

        Ball
            .Active
            .Subscribe(
                value =>
                {
                    gameObject.SetActive(value);
                })
            .AddTo(this);
    }

    public Ball Ball { get; set; } = new Ball(50, 1);

    private void DeactivateBall()
    {
        Ball.Active.Value = false;
        transform.position = new Vector3(-100f, -100f);
    }

    private void PutBallIntoPlay()
    {
        transform.parent = null;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Ball.InitialForce, Ball.InitialForce));
    }
}
