using UnityEngine;

namespace BreakoutGame
{
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 10f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rb.linearVelocity = transform.up * _speed;
        }
    }
}
