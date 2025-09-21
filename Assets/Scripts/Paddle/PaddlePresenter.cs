using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddlePresenter : MonoBehaviour
    {
        [MyConfig]
        [SerializeField]
        private Config _config;

        public Paddle Paddle { get; set; }

        public Transform InitialBallPosTrfm => _config._initialBallPosTrfm;

        // TODO: Remove this
        public class Dummy
        {
            public Dummy(PaddlePresenter view)
            {
                Debug.Log(view._config);
            }
        }

        private void Update() => Paddle.Tick(Time.deltaTime);

        private void Awake()
        {
            Paddle = new Paddle(gameObject, _config);
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Paddle.OnCollisionEnter2D(collision);
        //}

        [Serializable]
        public sealed class Config
        {
            public GameObject _ballPresenter;
            public Transform _initialBallPosTrfm;
            public Transform _graphicTrfm;
        }
    }
}