using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class ClampedMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider2D _leftWall;
    [SerializeField] private Collider2D _rightWall;

    private SpriteRenderer _renderer;
    private Mouse _mouse;
    //private PlayerInputActions input;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _mouse = Mouse.current;
    }

    private void Update()
    {
        Vector2 mousePos = _mouse.position.ReadValue();
        float mousePosWorldX = _camera.ScreenToWorldPoint(mousePos).x;
        float halfWidth = _renderer.bounds.extents.x;
        float minX = _leftWall.bounds.max.x + halfWidth;
        float maxX = _rightWall.bounds.min.x - halfWidth;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(mousePosWorldX, minX, maxX);
        transform.position = pos;
    }
}
