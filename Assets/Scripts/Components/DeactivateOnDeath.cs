using GameCtor.DevToolbox;
using System;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(Health))]
    public sealed class DeactivateOnDeathAction : MonoBehaviour
    {
        private void Start()
        {
            var health = GetComponent<Health>();
            Ensure.NotNull(health);
            //health.OnDied += OnDied;
        }

        private void OnDied()
        {
            gameObject.SetActive(false);
        }

        public void Execute(GameObject victim)
        {
            victim.SetActive(false);
        }
    }
}
