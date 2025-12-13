using GameCtor.DevToolbox;
using R3;
using R3.Triggers;
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
