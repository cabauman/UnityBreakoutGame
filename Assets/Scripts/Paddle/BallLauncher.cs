using System.Collections.Generic;
using R3;
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
        private readonly List<BallPresenter> _attachedBalls = new();

        private float MaxBounceAngleRad => _maxAngleDeg * Mathf.Deg2Rad;

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
            ball.AttachTo(transform);
        }

        private Vector2 CalculateBallLaunchForce(Vector2 point)
        {
            var deltaX = point.x - transform.position.x;
            float normalizedDistance = Mathf.Clamp(deltaX / (Width * 0.5f), -1f, 1f);
            // Convert to (0, 1) range for angle interpolation
            var normalizedLocalContactX = (normalizedDistance + 1f) * 0.5f; // or normalizedDistance * 0.5f + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + MaxBounceAngleRad,
                Mathf.PI / 2 - MaxBounceAngleRad,
                normalizedLocalContactX
            );

            var force = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle)) * _launchForce;
            return force;
        }

        private void LaunchBalls()
        {
            foreach (var ball in _attachedBalls)
            {
                ball.Trfm.parent = null;
                var force = CalculateBallLaunchForce(ball.Trfm.position);
                ball.SetForce(force);
            }

            _attachedBalls.Clear();
        }

        private void LaunchBalls2()
        {
            foreach (var ball in _attachedBalls)
            {
                ball.Trfm.parent = null;

                var force = PlaneBouncingUtility.CalculateBounceDirection(
                    transform.position,
                    Vector2.up,
                    Width,
                    ball.Trfm.transform.position,
                    MaxBounceAngleRad,
                    _launchForce
                );

                ball.SetForce(force);
            }
            _attachedBalls.Clear();
        }
    }
}