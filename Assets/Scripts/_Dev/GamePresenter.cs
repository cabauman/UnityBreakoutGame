using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class GamePresenter : MonoBehaviour
    {
        [Flatten]
        [SerializeField]
        private Config _config;

        private void Awake()
        {
            Game = new GameDraft(gameObject, _config);
        }

        private void Start()
        {
            Game.Start();
        }

        //private void Update()
        //{
        //    Game.Tick(Time.deltaTime);
        //}

        public GameDraft Game { get; private set; }

        [Serializable]
        public sealed class Config
        {
            [Header("Object References")]
            public Ball _ballPresenter;
            public Paddle _paddlePresenter;
            public List<Brick> _brickPresenters;
            public Ball _ballPresenterPrefab;

            [Header("Parameters")]
            public uint _defaultNumLives = 1;
        }
    }
}