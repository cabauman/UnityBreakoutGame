using Cysharp.Threading.Tasks;
using GameCtor.DevToolbox;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutGame
{
    public sealed class LevelTransitioner : MonoBehaviour
    {
        [SerializeField]
        private BallManager _ballManager;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private float _fadeDuration = 1.0f;

        [SerializeField]
        private BestTimeSaver _bestTimeSaver;

        [SerializeField]
        private TextMeshProUGUI _bestTimeLabel;

        private void Awake()
        {
            Ensure.NotNull(_ballManager);
            Ensure.NotNull(_image);
        }

        public async UniTask Enter()
        {
            await Tween.Alpha(_image, 0f, _fadeDuration);
        }

        public async UniTask Exit(LevelResult levelResult)
        {
            if (levelResult.Type == LevelResultType.None)
            {
                return;
            }

            _ballManager.ResetAll();
            await Tween.Alpha(_image, 1f, _fadeDuration);

            if (!_bestTimeSaver.SaveBestTime(levelResult.Level))
            {
                float bestTime = 1.23f; // Placeholder for actual best time retrieval
                _bestTimeLabel.text = $"New Best Time: {bestTime:F2} s";
                await Tween.Alpha(_bestTimeLabel, 0f, 1f, _fadeDuration, cycleMode: CycleMode.Yoyo, cycles: 2);
            }
        }
    }
}
