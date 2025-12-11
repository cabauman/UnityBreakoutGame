using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class UrlLinkFormatter : MonoBehaviour
    {
        private const string Pattern = @"(((http|ftp|https):\/\/)?[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
        private const string Replacement = "<#{0}><link=$1><u>$1</u></link></color>";

        [SerializeField]
        private Color32 _linkColor;

        private TextMeshProUGUI _textField;
        private string _linkColorHex;

        private void Awake()
        {
            _textField = GetComponent<TextMeshProUGUI>();
            _linkColorHex = ColorUtility.ToHtmlStringRGB(_linkColor);
        }

        public static string UpdateText(string text, string hexColor = "019DDA")
        {
            return Regex.Replace(
                text,
                Pattern,
                string.Format(Replacement, hexColor));
        }

        [ContextMenu("UpdateText")]
        public void UpdateText()
        {
            _textField.text = Regex.Replace(
                _textField.text,
                Pattern,
                string.Format(Replacement, _linkColorHex));
        }
    }
}
