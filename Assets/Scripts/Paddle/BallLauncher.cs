using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace BreakoutGame
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class BallLauncher : MonoBehaviour
    {
        [SerializeField]
        [Range(0.5f, 2f)]
        private float _launchForce = 1f;

        [SerializeField]
        [Range(0f, 89f)]
        private float _maxAngleDeg = 75f;

        private SpriteRenderer _spriteRenderer;
        private float _maxBounceAngleRad => _maxAngleDeg * Mathf.Deg2Rad;
        private List<BallPresenter> _attachedBalls = new();
        private List<RigidBody2D> _attachedBalls2 = new();

        private float Width => _spriteRenderer.bounds.size.x * transform.localScale.x;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Tick(float deltaTime)
        {
            if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                LaunchBalls();
            }
        }

        public void AttachBall(BallPresenter ball)
        {
            Assert.IsFalse(_attachedBalls.Contains(ball));
            _attachedBalls.Add(ball);
            ball.Trfm.parent = transform;
        }

        public void AttachBall(Rigidbody2D ball)
        {
            Assert.IsFalse(_attachedBalls2.Contains(ball));
            _attachedBalls2.Add(ball);
            ball.transform.parent = transform;
        }

        private Vector2 CalculateBallLaunchForce(Vector2 point)
        {
            var deltaX = point.x - transform.position.x;
            float normalizedDistance = Mathf.Clamp(deltaX / (Width * 0.5f), -1f, 1f);
            // Convert to (0, 1) range for angle interpolation
            var normalizedLocalContactX = (normalizedDistance + 1f) * 0.5f; // or normalizedDistance * 0.5f + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + _maxBounceAngleRad,
                Mathf.PI / 2 - _maxBounceAngleRad,
                normalizedLocalContactX
            );

            var force = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle)) * _launchForce;
            return force;
        }

        private void LaunchBalls()
        {
            // foreach (var ball in _attachedBalls)
            // {
            //     ball.Trfm.parent = null;
            //     var force = CalculateBallLaunchForce(this, ball.Trfm.position);
            //     ball.SetForce(force);
            // }
            _attachedBalls.Clear();
        }

        private void LaunchBalls2()
        {
            foreach (var ball in _attachedBalls2)
            {
                ball.transform.parent = null;
                var force = CalculateBallLaunchForce(ball.transform.position);
                ball.AddForce(force, ForceMode2D.Impulse);
            }
            _attachedBalls2.Clear();
        }
    }
}