using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PowerUpPresenter : MonoBehaviour
{
    private const string TAG_DEAD_ZONE = "DeadZone";

    private void Start()
    {
        this
            .OnTriggerEnter2DAsObservable()
            //.Where(collider => collider.tag == TAG_POWER_UP)
            .Select(collider => collider.GetComponent<PaddlePresenter>())
            .Where(x => x != null)
            .Do(paddlePresenter => PowerUp.ApplyEffect(paddlePresenter.Paddle))
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(this);

        this
            .OnTriggerEnter2DAsObservable()
            .Where(collider => collider.tag == TAG_DEAD_ZONE)
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(this);
    }

    public PowerUp PowerUp { get; set; }
}
