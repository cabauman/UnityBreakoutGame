using System.Linq;
using GameCtor.FuseDI;
using R3;
using UnityEngine;
using UnityEngine.Assertions;

namespace BreakoutGame
{
    public interface IBrickManager
    {
    }
    public sealed partial class BrickManager : MonoBehaviour, IPostInject, IBrickManager
    {
        [SerializeField]
        private Brick[] _bricks;

        [Inject] IRandom _random;

        public ReactiveProperty<int> BricksRemaining { get; private set; }

        public IRandom Random => _random;

        private void Awake()
        {
            Debug.Log("BrickManager Awake");
            BricksRemaining = new ReactiveProperty<int>(_bricks.Length);
        }

        //public void Inject(BreakoutGame.IRandom _random)
        //{
        //    Debug.Log("BrickManager Inject");
        //    this._random = _random;
        //}

        public void PostInject()
        {
            Debug.Log("BrickManager PostInject");
        }

        private void Start()
        {
            Debug.Log("BrickManager Start");
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

        public void MarkBricksAsTriggers(bool value)
        {
            // TODO: Only affect certain brick types.
            foreach (var brick in _bricks)
            {
                var collider = brick.GetComponent<BoxCollider2D>();
                Assert.IsNotNull(collider);
                collider.isTrigger = value;
            }
        }
    }
}