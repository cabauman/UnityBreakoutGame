using System.Collections.Generic;
using UnityEngine;

namespace BreakoutGame
{
    [CreateAssetMenu(fileName = "PowerUpTable", menuName = "BreakoutGame/PowerUpTable")]
    public sealed class PowerUpTable : ScriptableObject
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float _dropChance;
        public float DropChance => _dropChance;

        [Flatten]
        [SerializeField]
        private PowerUpConfig[] _configs;
        public IReadOnlyList<PowerUpConfig> Configs => _configs;
    }
}