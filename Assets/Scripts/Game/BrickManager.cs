using System.Linq;
using UniRx;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BrickManager : MonoBehaviour
    {
        [SerializeField]
        private Brick[] _bricks;

        public IReactiveProperty<int> BricksRemaining { get; private set; }

        private void Awake()
        {
            BricksRemaining = new ReactiveProperty<int>(_bricks.Length);

            _bricks
                .ToObservable()
                .SelectMany(x => x.Presenter.Active)
                .Where(active => active == false)
                .Subscribe(_ => BricksRemaining.Value -= 1);
        }

        public void ResetGame()
        {
            BricksRemaining.Value = _bricks.Length;

            foreach (var brick in _bricks)
            {
                brick.Presenter.ResetHp.Execute(Unit.Default);
            }
        }
    }
}