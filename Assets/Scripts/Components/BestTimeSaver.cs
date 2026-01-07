using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class BestTimeSaver : MonoBehaviour, IPostInject
    {
        [Inject]
        private LevelEvents _levelEvents;

        [SerializeField]
        private StopwatchUI _stopwatch;

        void IPostInject.PostInject()
        {
            Ensure.NotNull(_levelEvents);
            Ensure.NotNull(_stopwatch);

            //_levelEvents.LevelPassed
            //    .Subscribe(level => SaveBestTime(level))
            //    .AddTo(this);
        }

        public bool SaveBestTime(int level)
        {
            var bestTimeKey = $"BestTime_Level_{level}";
            ULog.Trace($"Checking best time for level {level}");
            var currentTime = _stopwatch.ElapsedSeconds;
            if (PlayerPrefs.HasKey(bestTimeKey))
            {
                var bestTime = PlayerPrefs.GetFloat(bestTimeKey);
                if (currentTime < bestTime)
                {
                    PlayerPrefs.SetFloat(bestTimeKey, currentTime);
                    ULog.Info($"New best time for level {level}: {currentTime} seconds");
                    return true;
                }
            }
            else
            {
                PlayerPrefs.SetFloat(bestTimeKey, currentTime);
                ULog.Info($"First recorded time for level {level}: {currentTime} seconds");
                return true;
            }

            return false;
        }
    }
}
