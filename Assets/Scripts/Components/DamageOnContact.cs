using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class DamageOnContact : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var other = collision.collider;
            Debug.Log($"DamageOnContact: {other.name}");
            if (other.TryGetComponent<Health>(out var health))
            {
                Debug.Log($"Applying {_damage} damage to {other.name}");
                health.Reduce(_damage);
            }
        }
    }
}
