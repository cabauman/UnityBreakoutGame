using UnityEngine;

namespace BreakoutGame
{
    public class ReverseBounceModifier : PowerUp
    {
        public override void ApplyEffect(Vector3 position)
        {
            Debug.Log("ReverseBounceModifier");
        }
    }
}