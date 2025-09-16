using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public class Ball
    {
        public Ball(float initialForce, int power, Vector3 startPosition)
        {
            InitialForce = initialForce;
            Power = power;
            StartPosition = startPosition;
            Active = new ReactiveProperty<bool>(true);
        }

        public float InitialForce { get; }

        public int Power { get; }

        public Vector3 StartPosition { get; }

        public IReactiveProperty<bool> Active { get; }

        public Vector2 GetInitialForce(float angleDeg)
        {
            return new Vector2
            {
                x = Mathf.Sin(angleDeg * Mathf.Deg2Rad) * InitialForce,
                y = Mathf.Cos(angleDeg * Mathf.Deg2Rad) * InitialForce
            };
        }

        public Vector2 CalculatePaddleBounceForce(float localContactX, float paddleWidth, float maxBounceAngleRad)
        {
            // Map the horizontal contact point to the (0, 1) range.
            // Input is in the range (-paddleWidth/2, paddleWidth/2)
            var normalizedLocalContactX = localContactX / paddleWidth + 0.5f;
            var bounceAngle = Mathf.Lerp(
                Mathf.PI / 2 + maxBounceAngleRad,
                Mathf.PI / 2 - maxBounceAngleRad,
                normalizedLocalContactX
            );

            return new Vector2
            {
                x = Mathf.Cos(bounceAngle) * InitialForce,
                y = Mathf.Sin(bounceAngle) * InitialForce
            };
        }
    }
}