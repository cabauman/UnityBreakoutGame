using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCtor.DevToolbox
{
    public sealed class FrameRateCounter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _display;

        [SerializeField]
        private DisplayMode _displayMode = DisplayMode.FPS;

        [SerializeField]
        [Range(0.1f, 2f)]
        private float _sampleDuration = 1f;

        [SerializeField]
        private Button _modeToggleButton;

        private int _frames;
        private float _duration;
        private float _bestDuration = float.MaxValue;
        private float _worstDuration;

        private void Awake()
        {
            _modeToggleButton.onClick.AddListener(() =>
            {
                _displayMode = _displayMode == DisplayMode.FPS ? DisplayMode.MS : DisplayMode.FPS;
            });
        }

        // TODO: Use UpdateManager
        private void Update()
        {
            float frameDuration = Time.unscaledDeltaTime;
            _frames += 1;
            _duration += frameDuration;

            if (frameDuration < _bestDuration)
            {
                _bestDuration = frameDuration;
            }
            if (frameDuration > _worstDuration)
            {
                _worstDuration = frameDuration;
            }

            if (_duration >= _sampleDuration)
            {
                if (_displayMode == DisplayMode.FPS)
                {
                    _display.SetText(
                        "FPS\n{0:0}\n{1:0}\n{2:0}",
                        1f / _bestDuration,
                        _frames / _duration,
                        1f / _worstDuration);
                }
                else
                {
                    _display.SetText(
                        "MS\n{0:1}\n{1:1}\n{2:1}",
                        1000f * _bestDuration,
                        1000f * _duration / _frames,
                        1000f * _worstDuration);
                }
                _frames = 0;
                _duration = 0f;
                _bestDuration = float.MaxValue;
                _worstDuration = 0f;
            }
        }

        public enum DisplayMode
        {
            FPS,
            MS,
        }
    }
}
