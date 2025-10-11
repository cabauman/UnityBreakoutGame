using UnityEngine;
using UnityEngine.Assertions;

namespace BreakoutGame
{
    public sealed class ClampedMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Collider2D _leftWall;
        [SerializeField] private Collider2D _rightWall;
        [SerializeField] private SpriteRenderer _renderer;

        private IPlayerInputProvider _playerInputProvider;

        private void Awake()
        {
            _playerInputProvider = GetComponent<IPlayerInputProvider>();
            Assert.IsNotNull(_playerInputProvider);
        }

        private void Update()
        {
            Vector2 mousePos = _playerInputProvider.GetHorizontalInput();
            if (mousePos == Vector2.zero)
            {
                return;
            }
            //Vector2 mousePos = _mouse.position.ReadValue();
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
