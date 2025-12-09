using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraBallPowerUp : PowerUp
    {
        public override string SpriteName { get; } = "ExtraBallPowerUp";

        public override void ApplyEffect(Game game, Vector3 position)
        {
            UnityEngine.Debug.Log("Extra ball!");
            game.CreateBonusBall.Execute(new Ball(50, 1, position));
        }
    }
}
