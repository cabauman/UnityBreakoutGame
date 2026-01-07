using UnityEngine;

namespace BreakoutGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class Brick : MonoBehaviour
    {
        [SerializeField]
        private int _initialHp = 1;
    }
}
