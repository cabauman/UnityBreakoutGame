using R3;
using R3.Triggers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BreakoutGame
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField] private int _hp = 1;
        public int Current => _hp;

        public event Action OnDied;
        public UnityEvent Destroyed;

        private void Start()
        {
            this.OnDisableAsObservable()
                .Subscribe(_ => Debug.Log("Disabled"));
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable called");
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
