using System.Collections.Generic;
using UnityEngine;

namespace BreakoutGame
{
    [CreateAssetMenu(fileName = "PowerUpTable", menuName = "BreakoutGame/PowerUpTable")]
    public sealed class PowerUpTable : ScriptableObject
    {
        //[MyConfig]
        [SerializeReference]
        private PowerUpConfig[] _configs;
        public IReadOnlyList<PowerUpConfig> Configs => _configs;
    }
}