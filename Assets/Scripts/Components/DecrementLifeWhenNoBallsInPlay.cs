using GameCtor.DevToolbox;
using GameCtor.FuseDI;
using R3;
using UnityEngine;

namespace BreakoutGame
{
    public sealed partial class DecrementLifeWhenNoBallsInPlay : MonoBehaviour, IPostInject
    {
        [SerializeField]
        private LifeTracker _lifeTracker;

        [SerializeField]
        private BallManager _ballManager;

        [Inject]
        private IReadOnlyLevelEvents _levelEvents;

        private void Start()
        {
            Ensure.NotNull(_lifeTracker);
            Ensure.NotNull(_ballManager);
            Ensure.NotNull(_levelEvents);

            _ballManager.NumBallsInPlay
                .Skip(1)
                .Where(x => x == 0 && _levelEvents.LevelReady.CurrentValue == true)
                .Subscribe(_ => _lifeTracker.RemoveLife())
                .AddTo(this);
        }
    }
}
