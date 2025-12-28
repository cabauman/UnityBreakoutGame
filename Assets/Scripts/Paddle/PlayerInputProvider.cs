using R3;
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
        [SerializeField] private GameManager _gameManager;

        private void Start()
        {
            _gameManager.GameLost.Subscribe(_ => this.enabled = false);
            _gameManager.GameWon.Subscribe(_ => this.enabled = false);
            _gameManager.GameStarted.Subscribe(_ => this.enabled = true);
        }

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