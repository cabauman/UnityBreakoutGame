using R3;
using TMPro;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class StopwatchUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private LevelManager _levelManager;

        [SerializeField]
        private float _refreshInterval = 0.1f;

        private bool _isRunning;
        private float _elapsedSeconds;
        private float _timeSinceLastUpdate;

        public float ElapsedSeconds => _elapsedSeconds;

        public void Begin()
        {
            _isRunning = true;
            _elapsedSeconds = 0f;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Resume()
        {
            _isRunning = true;
        }

        public void Tick(float dt)
        {
            if (!_isRunning)
            {
                return;
            }

            _elapsedSeconds += dt;

            _timeSinceLastUpdate += dt;
            if (_timeSinceLastUpdate < _refreshInterval)
            {
                return;
            }

            _timeSinceLastUpdate -= _refreshInterval;

            _label.text = $"{_elapsedSeconds:F2}s";
        }

        private void Start()
        {
            Begin();
            _levelManager.LevelStarted.Subscribe(_ => Begin());
            //_levelManager.LevelResumed.Subscribe(_ => Resume());
            //_levelManager.LifeLost.Subscribe(_ => Stop());
            _levelManager.LevelPassed.Subscribe(_ => Stop());
            _levelManager.LevelFailed.Subscribe(_ => Stop());
        }

        private void Update() => Tick(Time.deltaTime);
    }
}
