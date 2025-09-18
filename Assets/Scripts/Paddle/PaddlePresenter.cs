using System;
using UnityEngine;

namespace BreakoutGame
{
    public class PaddlePresenter : MonoBehaviour
    {
        [SerializeField]
        private Config _config;

        public class Dummy
        {
            public Dummy(PaddlePresenter view)
            {
                Debug.Log(view._config);
            }
        }

        private void Update() => Paddle.Tick(Time.deltaTime);

        public void Init()
        {
            Paddle = new Paddle(this, _config);

            //Observable
            //    .EveryUpdate()
            //    .Select(_ => Input.mousePosition)
            //    .Subscribe(UpdateXPosition)
            //    .AddTo(_view);

            //this
            //    .OnCollisionEnter2DAsObservable()
            //    .Where(collision => collision.gameObject.name == PADDLE_COLLIDER_NAME)
            //    .Subscribe(CalculateBounceVelocity)
            //    .AddTo(this);
        }

        public Paddle Paddle { get; private set; }

        [Serializable]
        public sealed class Config
        {
            public BallPresenter _ballPresenter;
            public Transform _initialBallPosTrfm;
            public Transform _graphicTrfm;
        }
    }
}