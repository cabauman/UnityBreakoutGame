using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public sealed partial class BallLauncher : MonoBehaviour, IPostInject
    {
        [SerializeField]
        private InputActionReference _launchInput;

        [SerializeField]
        private Transform _ballParent;

        [SerializeField]
        private float _launchSpeed = 6f;

        [Inject]
        private LevelEvents _levelEvents;

        public void Launch()
        {
            OnLaunchPerformed(default);
        }

        public bool IsAttached(Transform trfm)
        {
            return trfm.parent == _ballParent;
        }

        private void Awake()
        {
            Ensure.NotNull(_launchInput);
            Ensure.NotNull(_ballParent);
        }

        private void OnEnable()
        {
            _launchInput.action.Enable();
            _launchInput.action.performed += OnLaunchPerformed;
        }

        private void OnDisable()
        {
            _launchInput.action.Disable();
            _launchInput.action.performed -= OnLaunchPerformed;
        }

        void IPostInject.PostInject()
        {
            _levelEvents.LevelReady.Subscribe(value => this.enabled = value).AddTo(this);
        }

        private void OnLaunchPerformed(InputAction.CallbackContext context)
        {
            for (int i = _ballParent.childCount - 1; i >= 0; i--)
            {
                Transform ballTransform = _ballParent.GetChild(i);
                ballTransform.SetParent(null);

                if (ballTransform.TryGetComponent<Rigidbody2D>(out var rb))
                {
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
