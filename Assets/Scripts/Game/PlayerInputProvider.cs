using UnityEngine;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public sealed class PlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {
        [SerializeField] public InputActionReference _moveAction;
        [SerializeField] public InputActionReference _launchAction;

        private void OnEnable()
        {
            _moveAction.action.Enable();
            _launchAction.action.Enable();
        }

        private void OnDisable()
        {
            _moveAction.action.Disable();
            _launchAction.action.Disable();
        }

        public Vector2 GetHorizontalInput()
        {
            _moveAction.action.Enable();
            return _moveAction.action.ReadValue<Vector2>();
        }

        public bool IsLaunchPressed()
        {
            return _launchAction.action.triggered;
        }
    }
}