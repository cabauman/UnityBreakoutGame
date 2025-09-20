﻿using System;
using System.ComponentModel;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    public sealed class BallPresenter : MonoBehaviour, IView, ITestable<BallPresenter.Config>
    {
        [SerializeField]
        private Config _config;

        private void Awake()
        {
            Ball = new Ball(gameObject, _config);
        }

        void ITestable<Config>.SetConfig(Config config) => _config = config;

        public Ball Ball { get; set; }

        public GameObject GameObject => gameObject;

        [Serializable]
        public sealed class Config
        {
            [Range(0f, 100f)]
            public float _initialForce = 50f;

            // TODO: Rename to _initialAngleDeg
            [Range(-90f, 90f)]
            public float _initialAngle = 45f;

            [Tooltip("How much damage I can inflict on a brick per collision.")]
            public int _power = 1;

            [Range(0f, 90f)]
            [Tooltip("0: completely vertical, 90: +/- 90degrees from the up vector")]
            public float _maxPaddleBounceAngle = 75f;
        }
    }
}