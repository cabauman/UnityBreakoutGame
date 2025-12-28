using UnityEngine;

namespace BreakoutGame
{
    public sealed class Stopwatch : MonoBehaviour
    {
        private float _elapsedTime;
        private bool _isRunning;

        public float ElapsedTime => _elapsedTime;

        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Reset()
        {
            _elapsedTime = 0f;
        }

        private void Update()
        {
            if (_isRunning)
            {
                _elapsedTime += Time.deltaTime;
            }
        }
    }
}
