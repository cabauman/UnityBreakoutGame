using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace GameCtor.DevToolbox
{
    public sealed class ProfilerStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Button _copyButton;
        [SerializeField] private int _updateIntervalSeconds = 1;

        private ProfilerRecorder _totalUsedMemoryRecorder;
        private ProfilerRecorder _totalReservedMemoryRecorder;
        private ProfilerRecorder _gcUsedMemoryRecorder;
        private ProfilerRecorder _gcReservedMemoryRecorder;
        private ProfilerRecorder _gfxUsedMemoryRecorder;
        private ProfilerRecorder _gfxReservedMemoryRecorder;
        private ProfilerRecorder _systemUsedMemoryRecorder;

        private ProfilerRecorder _textureCountRecorder;
        private ProfilerRecorder _textureMemoryRecorder;
        private ProfilerRecorder _meshCountRecorder;
        private ProfilerRecorder _meshMemoryRecorder;
        private ProfilerRecorder _materialCountRecorder;
        private ProfilerRecorder _materialMemoryRecorder;
        private ProfilerRecorder _gameObjectsInScenesRecorder;
        private ProfilerRecorder _drawCallsCountRecorder;

        private float _elapsed;
        private StringBuilder _sb = new(1024);

        private void Start()
        {
            _copyButton.onClick.AddListener(() =>
            {
                var timestamp = System.DateTime.Now;
                GUIUtility.systemCopyBuffer = $"{timestamp:yyyy-MM-dd HH:mm:ss}\n{_label.text}";
            });

            _totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
            _totalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
            _gcUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory");
            _gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
            _gfxUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory");
            _gfxReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Reserved Memory");
            _systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
#if DEBUG
            _textureCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Count");
            _textureMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory");
            _meshCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Count");
            _meshMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Memory");
            _materialCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Count");
            _materialMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Memory");
            _gameObjectsInScenesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Game Object Count");
            _drawCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
#endif
        }

        private void OnDestroy()
        {
            _totalReservedMemoryRecorder.Dispose();
            _totalUsedMemoryRecorder.Dispose();
            _gcUsedMemoryRecorder.Dispose();
            _gcReservedMemoryRecorder.Dispose();
            _gfxUsedMemoryRecorder.Dispose();
            _gfxReservedMemoryRecorder.Dispose();
            _systemUsedMemoryRecorder.Dispose();
#if DEBUG
            _textureCountRecorder.Dispose();
            _textureMemoryRecorder.Dispose();
            _meshCountRecorder.Dispose();
            _meshMemoryRecorder.Dispose();
            _materialCountRecorder.Dispose();
            _materialMemoryRecorder.Dispose();
            _gameObjectsInScenesRecorder.Dispose();
            _drawCallsCountRecorder.Dispose();
#endif
        }

        private void Update()
        {
            _elapsed += Time.unscaledDeltaTime;
            if (_elapsed < _updateIntervalSeconds) return;
            _elapsed -= _updateIntervalSeconds;

            _sb.Clear();
            _sb.AppendFormat("Total Reserved Memory: {0}", FileSizeUtil.Format(_totalReservedMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("Total Used Memory: {0}", FileSizeUtil.Format(_totalUsedMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("GC Used Memory: {0}", FileSizeUtil.Format(_gcUsedMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("GC Reserved Memory: {0}", FileSizeUtil.Format(_gcReservedMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("Gfx Used Memory: {0}", FileSizeUtil.Format(_gfxUsedMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("Gfx Reserved Memory: {0}", FileSizeUtil.Format(_gfxReservedMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("System Used Memory: {0}", FileSizeUtil.Format(_systemUsedMemoryRecorder.LastValue));
#if DEBUG
            _sb.AppendLine().AppendLine();
            _sb.AppendFormat("Texture Count: {0}", _textureCountRecorder.LastValue);
            _sb.AppendLine();
            _sb.AppendFormat("Texture Memory: {0}", FileSizeUtil.Format(_textureMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("Mesh Count: {0}", _meshCountRecorder.LastValue);
            _sb.AppendLine();
            _sb.AppendFormat("Mesh Memory: {0}", FileSizeUtil.Format(_meshMemoryRecorder.LastValue));
            _sb.AppendLine();
            _sb.AppendFormat("Material Count: {0}", _materialCountRecorder.LastValue);
            _sb.AppendLine();
            _sb.AppendFormat("Material Memory: {0}", FileSizeUtil.Format(_materialMemoryRecorder.LastValue));
            _sb.AppendLine().AppendLine();
            _sb.AppendFormat("Game Object Count: {0}", _gameObjectsInScenesRecorder.LastValue);
            _sb.AppendLine();
            _sb.AppendFormat("Draw Calls Count: {0}", _drawCallsCountRecorder.LastValue);
#endif
            _label.text = _sb.ToString();
        }
    }
}
