using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BreakoutGame
{
    public interface IPlayerInputProvider
    {
        Vector2 GetHorizontalInput();
    }

    // TODO: Consider removing this abstraction
    public sealed partial class PlayerInputProvider : MonoBehaviour, IPlayerInputProvider, IPostInject
    {
        [SerializeField]
        private InputActionReference _moveAction;

        [Inject]
        private GameEvents _gameEvents;

        [Inject]
        private LevelEvents _levelEvents;

        private void Awake()
        {
            Ensure.NotNull(_moveAction);
        }

        private void OnEnable()
        {
            _moveAction.action.Enable();
        }

        private void OnDisable()
        {
            _moveAction.action.Disable();
        }

        void IPostInject.PostInject()
        {
            Ensure.NotNull(_gameEvents);
            Ensure.NotNull(_levelEvents);

            _levelEvents.LevelReady.Subscribe(isReady => this.enabled = isReady);
            _gameEvents.IsPaused.Subscribe(isPaused => this.enabled = !isPaused);
        }

        public Vector2 GetHorizontalInput()
        {
            return _moveAction.action.ReadValue<Vector2>();
        }
    }
}