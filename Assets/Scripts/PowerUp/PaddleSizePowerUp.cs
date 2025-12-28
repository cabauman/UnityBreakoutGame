using GameCtor.DevToolbox;
using R3;
using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class PaddleSizePowerUp : PowerUpPresenter
    {
        private readonly float _widthMultiplier = 1.5f;
        private readonly float _effectDuration = 5f;

        public override void ApplyEffect(GameObject go)
        {
            ULog.Trace("Paddle size increased!");
            var renderer = go.GetComponentInChildren<SpriteRenderer>();
            var scale = renderer.transform.localScale;
            scale.x *= _widthMultiplier;
            renderer.transform.localScale = scale;
            Observable
                .Timer(TimeSpan.FromSeconds(_effectDuration))
                .Subscribe(_ =>
                {
                    scale = renderer.transform.localScale;
                    scale.x /= _widthMultiplier;
                    renderer.transform.localScale = scale;
                });

            //target.AddComponent<IncreaseWidthModifier>().Duration = _effectDuration;
        }
    }

    // Or IncreaseWidthAttachment
    // Or IncreaseWidthEffect
    // Or IncreaseWidthBehavior
    public class IncreaseWidthModifier : TemporaryEffect
    {
        protected void OnEnable()
        {
            transform.localScale *= 1.3f;
        }

        protected void OnDestroy()
        {
            transform.localScale /= 1.3f;
        }
    }

    // public class IncreaseWidthPowerUp : MonoBehaviour, IPowerUpItem
    // {
    //     public void Apply(GameObject target)
    //     {
    //         target.AddComponent<IncreaseWidthModifier>().Duration = 10f;
    //     }
    // }
}
