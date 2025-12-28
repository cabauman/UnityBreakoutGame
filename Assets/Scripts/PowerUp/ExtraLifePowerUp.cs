using GameCtor.DevToolbox;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class ExtraLifePowerUp : PowerUpPresenter
    {
        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("Extra life!");
            var lifeTracker = GameObject.FindAnyObjectByType<LifeTracker>();
            Ensure.NotNull(lifeTracker);
            lifeTracker.AddLife();
        }
    }
}
