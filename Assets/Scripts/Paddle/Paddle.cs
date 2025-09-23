using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Paddle : MonoBehaviour
    {
        [MyConfig]
        [SerializeField]
        private Config _config;

        public PaddlePresenter Presenter { get; set; }

        public Transform InitialBallPosTrfm => _config._initialBallPosTrfm;

        // TODO: Remove this
        public class Dummy
        {
            public Dummy(Paddle view)
            {
                Debug.Log(view._config);
            }
        }

        private void Update() => Presenter.Tick(Time.deltaTime);

        private void Awake()
        {
            Presenter = new PaddlePresenter(gameObject, _config);
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