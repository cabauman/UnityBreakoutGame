using UnityEngine;

namespace GameCtor.DevToolbox
{
    [CreateAssetMenu(fileName = "ColorVariable", menuName = "DevToolbox/ScriptableObjects/ColorVariable", order = 1)]
    public sealed class ColorVariable : ScriptableObject
    {
        [SerializeField] private Color _value;
        public Color Value => _value;
    }
}
