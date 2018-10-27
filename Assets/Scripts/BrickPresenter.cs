using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BrickPresenter : MonoBehaviour
{
    [SerializeField]
    private Brick _brick;

    private void Start()
    {
        this
            .OnCollisionEnter2DAsObservable()
            .Select(collision => collision.collider.GetComponent<BallPresenter>().Ball)
            .Subscribe(ball => _brick.DecreaseHp.Execute(ball))
            .AddTo(this);

        _brick
            .Active
            .Subscribe(value => gameObject.SetActive(value))
            .AddTo(this);
    }

    public Brick Brick => _brick;
}
