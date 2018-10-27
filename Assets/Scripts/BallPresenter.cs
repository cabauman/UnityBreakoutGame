using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallPresenter : MonoBehaviour
{
    [SerializeField]
    private Ball _ball;

    private void Start()
    {
        Observable
            .EveryUpdate()
            .Where(_ => Input.GetButtonDown("Fire1")/* && ballInPlay*/)
            .Subscribe(
                _ =>
                {
                    transform.parent = null;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(_ball.InitialForce, _ball.InitialForce));
                })
            .AddTo(this);

        this
            .OnTriggerEnter2DAsObservable()
            .Where(collider => collider.tag == "DeadZone")
            .Subscribe(_ => _ball.Active.Value = false)
            .AddTo(this);

        _ball
            .Active
            .Subscribe(value => gameObject.SetActive(value))
            .AddTo(this);
    }

    public Ball Ball => _ball;
}
