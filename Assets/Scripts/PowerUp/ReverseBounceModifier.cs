using UnityEngine;

namespace BreakoutGame
{
    public class ReverseBounceModifier : PowerUp
    {
        public override string SpriteName { get; } = "ReverseBounceModifier";

        public override void ApplyEffect(Game game, Vector3 position)
        {
            Debug.Log("ReverseBounceModifier");
            //
        }
    }
}