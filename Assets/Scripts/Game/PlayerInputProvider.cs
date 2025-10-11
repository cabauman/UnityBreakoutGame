using UnityEngine;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public sealed class PlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {
        [SerializeField] public InputActionReference _moveAction;
        [SerializeField] public InputActionReference _launchAction;

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