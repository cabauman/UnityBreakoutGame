using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ExtraLifePowerUp : PowerUp
    {
        [Inject]
        private LifeTracker _lifeTracker;

        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("");
            Ensure.NotNull(_lifeTracker);
            _lifeTracker.AddLife();
        }
    }
}
