using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField]
        private Config _config;

        private void Awake()
        {
            Game = new Game(gameObject, _config);
        }

        private void Start()
        {
            Game.Start();
        }

        //private void Update()
        //{
        //    Game.Tick(Time.deltaTime);
        //}

        public Game Game { get; private set; }

        [Serializable]
        public sealed class Config
        {
            [Header("Object References")]
            public BallPresenter _ballPresenter;
            public PaddlePresenter _paddlePresenter;
            public List<BrickPresenter> _brickPresenters;
            public BallPresenter _ballPresenterPrefab;

            [Header("Parameters")]
            public uint _defaultNumLives = 1;
        }
    }
}