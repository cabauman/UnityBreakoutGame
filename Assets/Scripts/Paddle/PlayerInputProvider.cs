using UnityEngine;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public interface IPlayerInputProvider
    {
        Vector2 GetHorizontalInput();
        bool IsLaunchPressed();
    }

    public sealed class PlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _launchAction;

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
            return _moveAction.action.ReadValue<Vector2>();
        }

        public bool IsLaunchPressed()
        {
            return _launchAction.action.triggered;
        }
    }
}