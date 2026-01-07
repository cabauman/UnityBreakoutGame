using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(Health))]
    public sealed class DeactivateOnDeathAction : MonoBehaviour
    {
        public void Execute(GameObject victim)
        {
            victim.SetActive(false);
        }

        private void Awake()
        {
            var health = GetComponent<Health>();
            Ensure.NotNull(health);
            //health.OnDied += OnDied;
        }

        private void OnDied()
        {
            gameObject.SetActive(false);
        }
    }
}
