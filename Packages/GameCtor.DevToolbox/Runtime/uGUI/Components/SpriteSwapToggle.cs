using UnityEngine;
using UnityEngine.UI;

namespace GameCtor.DevToolbox
{
    [RequireComponent(typeof(Toggle))]
    public sealed class SpriteSwapToggle : MonoBehaviour
    {
        [SerializeField] private Sprite _selectedSprite;

        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void Start()
        {
            _toggle.toggleTransition = Toggle.ToggleTransition.None;
            _toggle.onValueChanged.AddListener(ToggleSprite);
            ToggleSprite(_toggle.isOn);
        }

        public void SetIsOnWithoutNotify(bool value)
        {
            _toggle.SetIsOnWithoutNotify(value);
            ToggleSprite(value);
        }

        private void ToggleSprite(bool value)
        {
            _toggle.image.overrideSprite = value ? _selectedSprite : null;
        }
    }
}
