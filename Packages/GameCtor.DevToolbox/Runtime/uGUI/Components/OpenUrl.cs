using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCtor.DevToolbox
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class OpenUrl : MonoBehaviour, IPointerClickHandler
    {
        private TextMeshProUGUI _textField;

        private void Awake()
        {
            _textField = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textField, Input.mousePosition, null);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = _textField.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }
}
