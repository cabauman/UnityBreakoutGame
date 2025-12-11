using UnityEngine;

namespace GameCtor.DevToolbox
{
    [CreateAssetMenu(fileName = "StringVariable", menuName = "DevToolbox/ScriptableObjects/StringVariable", order = 1)]
    public sealed class StringVariable : ScriptableObject
    {
        [SerializeField] private string _value;
        public string Value => _value;
    }
}
