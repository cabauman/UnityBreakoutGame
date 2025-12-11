using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(IPlayerInputProvider))]
    public sealed class ClampedMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Collider2D _leftWall;
        [SerializeField] private Collider2D _rightWall;
        [SerializeField] private SpriteRenderer _renderer;

        private IPlayerInputProvider _playerInputProvider;

        private void Awake()
        {
            Ensure.NotNull(_camera);
            Ensure.NotNull(_leftWall);
            Ensure.NotNull(_rightWall);
            Ensure.NotNull(_renderer);

            _playerInputProvider = GetComponent<IPlayerInputProvider>();
            Ensure.NotNull(_playerInputProvider);
        }

        private void Update()
        {
            Vector2 mousePos = _playerInputProvider.GetHorizontalInput();
            if (mousePos == Vector2.zero)
            {
                return;
            }

            float mousePosWorldX = _camera.ScreenToWorldPoint(mousePos).x;
            float halfWidth = _renderer.bounds.extents.x;
            float minX = _leftWall.bounds.max.x + halfWidth;
            float maxX = _rightWall.bounds.min.x - halfWidth;

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(mousePosWorldX, minX, maxX);
            transform.position = pos;
        }
    }
}
