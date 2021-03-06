﻿using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class BallPresenter : MonoBehaviour
{
    private const string PADDLE_COLLIDER_NAME = "PaddleGraphic";

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
            .Where(collider => collider.tag == Tags.DEAD_ZONE)
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

    public Vector2 Velocity => _rigidbody.velocity;

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

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(bounceForce);
    }
}
