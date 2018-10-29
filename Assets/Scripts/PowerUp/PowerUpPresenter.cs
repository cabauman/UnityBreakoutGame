﻿using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PowerUpPresenter : MonoBehaviour
{
    private void Start()
    {
        this
            .OnTriggerEnter2DAsObservable()
            .Select(collider => collider.GetComponent<PaddlePresenter>())
            .Where(x => x != null)
            .Subscribe(_ => ApplyAndDestroy())
            .AddTo(this);

        this
            .OnTriggerEnter2DAsObservable()
            .Where(collider => collider.tag == Tags.DEAD_ZONE)
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(this);
    }

    public PowerUp PowerUp { get; set; }

    private void ApplyAndDestroy()
    {
        var gamePresenter = FindObjectOfType<GamePresenter>();
        PowerUp.ApplyEffect(gamePresenter.Game, transform.position);
        Destroy(gameObject);
    }
}
