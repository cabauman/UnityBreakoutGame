using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCtor.DevToolbox
{
    [RequireComponent(typeof(Toggle))]
    public sealed class ColorChangeToggle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Color _colorOn;
        [SerializeField] private Color _colorOff;
        [SerializeField] private Color _textColorOn;
        [SerializeField] private Color _textColorOff;

        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            _toggle.onValueChanged.AddListener(ToggleSprite);
            ToggleSprite(_toggle.isOn);
        }

        private void ToggleSprite(bool isOn)
        {
            if (isOn)
            {
                _label.color = _textColorOn;
                _toggle.targetGraphic.color = _colorOn;
            }
            else
            {
                _label.color = _textColorOff;
                _toggle.targetGraphic.color = _colorOff;
            }
        }
    }
}
