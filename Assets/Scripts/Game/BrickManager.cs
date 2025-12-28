using GameCtor.DevToolbox;
using log4net.Util;
using R3;
using R3.Triggers;
using System;
using UnityEngine;

namespace BreakoutGame
{
    public sealed class BrickManager : MonoBehaviour
    {
        private void Awake()
        {
            var bricks = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                bricks[i] = transform.GetChild(i).gameObject;
            }

            NumBricks = bricks.ToObservable()
                .SelectMany(static x => x.OnDisableAsObservable().Select(y => -1))
                .Scan(transform.childCount, static (acc, delta) => acc + delta)
                .ToReadOnlyReactiveProperty(transform.childCount);

            NumBricks.Subscribe(numBricks =>
            {
                ULog.Trace($"Number of bricks remaining: {numBricks}");
            });
        }

        public ReadOnlyReactiveProperty<int> NumBricks { get; private set; }
    }
}
