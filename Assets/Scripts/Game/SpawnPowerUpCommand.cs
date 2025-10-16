using GameCtor.FuseDI;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class SpawnPowerUpCommand : MonoCommand
    {
        [SerializeField] private PowerUpTable _powerUpTable;
        [Inject] private IPowerUpSpawner _powerUpSpawner;

        public override void Execute()
        {
            _powerUpSpawner.SpawnPowerUp(_powerUpTable, transform.position);
        }
    }
}