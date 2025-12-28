using GameCtor.DevToolbox;
using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class DamageOnContact : MonoBehaviour
    {
        [SerializeField]
        private int _damage = 1;

        [SerializeField]
        private Power _power;

        private void Awake()
        {
            Ensure.NotNull(_power);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var other = collision.collider;
            ULog.Trace($"DamageOnContact: {other.name}");
            if (other.TryGetComponent<Health>(out var health))
            {
                ULog.Trace($"Applying {_power.Value} damage to {other.name}");
                health.Reduce(_power.Value);
            }
        }
    }
}
