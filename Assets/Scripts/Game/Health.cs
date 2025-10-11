using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHp = 1;
        private int _currentHp;

        public int CurrentHp => _currentHp;
        public int MaxHp => _maxHp;

        private void Awake()
        {
            _currentHp = _maxHp;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");
            }

            _currentHp -= damage;
            if (_currentHp < 0) _currentHp = 0;
            if (_currentHp == 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void ResetHealth()
        {
            _currentHp = _maxHp;
            gameObject.SetActive(true);
        }
    }
}