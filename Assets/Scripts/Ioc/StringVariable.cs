using UnityEngine;

namespace GameCtor.DevToolbox
{
    [CreateAssetMenu(fileName = "StringVariable", menuName = "DevToolbox/ScriptableObjects/StringVariable", order = 1)]
    public sealed class StringVariable : ScriptableObject
    {
        [SerializeField] private string value;
        public string Value => value;
    }
}
