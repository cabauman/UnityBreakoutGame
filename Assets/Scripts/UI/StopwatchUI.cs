using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using TMPro;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class StopwatchUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private float _refreshInterval = 0.1f;

        [Inject]
        private LevelEvents _levelEvents;

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

        private void Awake()
        {
            ULog.Trace("");
        }

        private void Start()
        {
            ULog.Trace("");
            //Begin();
            _levelEvents.LevelStarted.Subscribe(_ => Begin());
            //_levelManager.LevelResumed.Subscribe(_ => Resume());
            //_levelManager.LifeLost.Subscribe(_ => Stop());
            _levelEvents.LevelPassed.Subscribe(_ => Stop());
            _levelEvents.LevelFailed.Subscribe(_ => Stop());
        }

        private void Update() => Tick(Time.deltaTime);
    }
}
