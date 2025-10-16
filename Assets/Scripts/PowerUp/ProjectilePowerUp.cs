using GameCtor.FuseDI;
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

    public sealed class ProjectileGameWorldEffect : IGameWorldEffect
    {
        private readonly ProjectileCollisionStrategyDecorator _strategy;

        public ProjectileGameWorldEffect(ProjectileCollisionStrategyDecorator strategy)
        {
            _strategy = strategy;
        }

        public void OnEnter(PaddlePresenter paddle)
        {
            Debug.Log("ProjectilePowerUp");
            paddle.SetBallCollisionStrategy(_strategy);
        }

        public void OnExit(PaddlePresenter paddle)
        {
            paddle.ResetBallCollisionStrategy();
        }
    }
}