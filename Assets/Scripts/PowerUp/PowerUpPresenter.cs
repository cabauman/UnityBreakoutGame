using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace BreakoutGame
{
    public class PowerUpPresenter : MonoBehaviour
    {
        private PowerUpConfig _config;

        private void Start()
        {
            this
                .OnTriggerEnter2DAsObservable()
                .Subscribe(ApplyAndDestroy)
                .AddTo(this);
        }

        //protected abstract void ApplyEffect(PaddlePresenter paddle);

        private void ApplyAndDestroy(Collider2D collider)
        {
            if (collider.CompareTag(Tags.DEAD_ZONE))
            {
                Destroy(gameObject);
                return;
            }

            if (collider.TryGetComponent<PaddlePresenter>(out var paddle))
            {
                //ApplyEffect(paddle);
                _config.ApplyEffect();
                Destroy(gameObject);
            }
        }

        public void Init(PowerUpConfig config)
        {
            _config = config;
        }
    }
}