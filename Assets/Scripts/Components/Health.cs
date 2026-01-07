using System;
using UnityEngine;
using UnityEngine.Events;

namespace BreakoutGame
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField]
        private int _hp = 1;

        private int _maxHp;

        public int Current => _hp;

        // TODO: Choose one
        public event Action OnDied;
        public UnityEvent Destroyed;

        private void Awake()
        {
            _maxHp = _hp;
        }

        private void OnDisable()
        {
            _hp = _maxHp;
        }

        public void Reduce(int amount)
        {
            _hp -= amount;
            if (_hp <= 0)
            {
                _hp = 0;
                OnDied?.Invoke();
                Destroyed.Invoke();
            }
        }
    }
}
