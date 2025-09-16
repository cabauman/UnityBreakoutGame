using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace BreakoutGame
{
    public sealed class BrickManager
    {
        public BrickManager(IReadOnlyList<Brick> bricks)
        {
            Bricks = bricks;
            BricksRemaining = new ReactiveProperty<int>(Bricks.Count);

            Bricks
                .ToObservable()
                .SelectMany(x => x.Active)
                .Where(active => active == false)
                .Subscribe(_ => BricksRemaining.Value -= 1);
        }

        public IReadOnlyList<Brick> Bricks { get; }

        public IReactiveProperty<int> BricksRemaining { get; }
    }
}