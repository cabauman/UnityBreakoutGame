using UniDig;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class HeavyBallPowerUp : PowerUp
    {
        [Inject] private HeavyBallGameWorldEffect _gameWorldEffect;

        protected override void ApplyEffect(Paddle paddle)
        {
            Debug.Log("HeavyBallPowerUp");
            paddle.Presenter.SetGameWorldEffect(_gameWorldEffect);
        }
    }

    public sealed class HeavyBallGameWorldEffect : IGameWorldEffect
    {
        private readonly BrickManager _brickManager;

        public HeavyBallGameWorldEffect(BrickManager brickManager)
        {
            _brickManager = brickManager;
        }

        public void OnEnter(PaddlePresenter paddle)
        {
            _brickManager.MarkBricksAsTriggers(true);
        }

        public void OnExit(PaddlePresenter paddle)
        {
            _brickManager.MarkBricksAsTriggers(false);
        }
    }
}