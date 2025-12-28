using GameCtor.DevToolbox;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BestTimeSaver : MonoBehaviour
    {
        [SerializeField]
        private IReadOnlyLevelEvents _levelEvents;

        [SerializeField]
        private LevelManager _levelManager;

        [SerializeField]
        private StopwatchUI _stopwatch;

        public ReactiveProperty<float> BestTime { get; } = new();

        private void Start()
        {
            //_levelEvents.LevelPassed
            //    .Subscribe(level => SaveBestTime(level))
            //    .AddTo(this);

            _levelManager.LevelPassed
                .Subscribe(level => SaveBestTime(level))
                .AddTo(this);
        }

        private void SaveBestTime(int level)
        {
            var bestTimeKey = $"BestTime_Level_{level}";
            Debug.Log($"Checking best time for level {level}");
            var currentTime = _stopwatch.ElapsedSeconds;
            if (PlayerPrefs.HasKey(bestTimeKey))
            {
                var bestTime = PlayerPrefs.GetFloat(bestTimeKey);
                if (currentTime < bestTime)
                {
                    PlayerPrefs.SetFloat(bestTimeKey, currentTime);
                    ULog.Info($"New best time for level {level}: {currentTime} seconds");
                }
            }
            else
            {
                PlayerPrefs.SetFloat(bestTimeKey, currentTime);
                ULog.Info($"First recorded time for level {level}: {currentTime} seconds");
            }
        }
    }
}
