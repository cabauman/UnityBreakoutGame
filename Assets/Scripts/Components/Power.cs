using UnityEngine;

namespace BreakoutGame
{
    public sealed class Power : MonoBehaviour
    {
        [SerializeField]
        private int _value = 1;

        public int Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
