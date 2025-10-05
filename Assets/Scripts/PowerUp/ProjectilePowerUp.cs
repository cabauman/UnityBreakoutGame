using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class ProjectilePowerUp : PowerUp
    {
        [SerializeField]
        private GameObject _prefab;

        [Inject] private ProjectileCollisionStrategyDecorator _strategy;

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("ProjectilePowerUp");
            paddle.Presenter.SetBallCollisionStrategy(_strategy);
        }
    }
}