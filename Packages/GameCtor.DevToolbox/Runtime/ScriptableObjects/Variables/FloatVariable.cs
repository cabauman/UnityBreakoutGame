using UnityEngine;

namespace GameCtor.DevToolbox
{
    [CreateAssetMenu(fileName = "FloatVariable", menuName = "DevToolbox/ScriptableObjects/FloatVariable", order = 1)]
    public sealed class FloatVariable : ScriptableObject
    {
        [SerializeField] private float _value;
        public float Value => _value;
    }
}
