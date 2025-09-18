using UnityEngine;

namespace BreakoutGame
{
    public class ExtraBallPowerUp : PowerUp
    {
        public override string SpriteName { get; } = "ExtraBallPowerUp";

        public override void ApplyEffect(Game game, Vector3 position)
        {
            Debug.Log("Extra ball!");
            game.CreateBonusBall.Execute(position);
        }
    }
}