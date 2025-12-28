using GameCtor.DevToolbox;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public sealed class BallLauncher : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _launchInput;

        [SerializeField]
        private Transform _ballParent;

        [SerializeField]
        private float _launchSpeed = 6f;

        public void Launch()
        {
            OnLaunchPerformed(default);
        }

        public bool IsAttached(Transform trfm)
        {
            return trfm.parent == _ballParent;
        }

        private void OnEnable()
        {
            Ensure.NotNull(_launchInput);
            Ensure.NotNull(_ballParent);
            _launchInput.action.performed += OnLaunchPerformed;
        }

        private void OnDisable()
        {
            _launchInput.action.performed -= OnLaunchPerformed;
        }

        private void OnLaunchPerformed(InputAction.CallbackContext context)
        {
            for (int i = _ballParent.childCount - 1; i >= 0; i--)
            {
                Transform ballTransform = _ballParent.GetChild(i);
                ballTransform.SetParent(null);
                if (ballTransform.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    //rb.linearVelocity = _velocity;
                    var planeWidth = GetComponentInChildren<Collider2D>().bounds.size.x;
                    var direction = PlaneBouncingUtility.CalculateBounceDirection(
                        planeCenter: transform.position,
                        planeNormal: Vector2.up,
                        planeWidth: planeWidth,
                        contactPoint: ballTransform.position,
                        maxBounceAngle: 75f * Mathf.Deg2Rad);

                    rb.linearVelocity = direction * _launchSpeed;
                }
            }
        }
    }
}
