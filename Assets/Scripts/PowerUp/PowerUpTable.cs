using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpTable", menuName = "BreakoutGame/PowerUpTable")]
public sealed class PowerUpTable : ScriptableObject
{
    [SerializeReference]
    private PowerUpConfig[] _configs;
    public IReadOnlyList<PowerUpConfig> Configs => _configs;
}
