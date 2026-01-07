using UnityEngine;

namespace BreakoutGame
{
    public class TemporaryEffect : MonoBehaviour
    {
        public float Duration;
        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration) Destroy(this);
        }
    }
}
