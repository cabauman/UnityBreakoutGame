using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    public sealed class CoroutineUtil
    {
        private readonly Dictionary<float, WaitForSeconds> _wfsMap = new(FloatComparer.Instance);
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new();
        private readonly WaitForFixedUpdate _waitForFixedUpdate = new();
        private readonly ConcurrentBag<WaitForSecondsRealtime> _pool = new();

        public WaitForSeconds WaitForSeconds(float seconds)
        {
            if (!_wfsMap.TryGetValue(seconds, out WaitForSeconds waitForSeconds))
            {
                waitForSeconds = new WaitForSeconds(seconds);
                _wfsMap.Add(seconds, waitForSeconds);
            }

            return waitForSeconds;
        }

        public WaitForEndOfFrame WaitForEndOfFrame() => _waitForEndOfFrame;

        public WaitForFixedUpdate WaitForFixedUpdate() => _waitForFixedUpdate;

        public WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
        {
            if (!_pool.TryTake(out var waitForSeconds))
            {
                waitForSeconds = new WaitForSecondsRealtime(seconds);
            }

            return waitForSeconds;
        }

        public void ReturnWaitForSecondsRealtime(WaitForSecondsRealtime waitForSeconds)
        {
            _pool.Add(waitForSeconds);
        }

        private sealed class FloatComparer : IEqualityComparer<float>
        {
            public static FloatComparer Instance { get; } = new();

            public bool Equals(float x, float y)
            {
                return Mathf.Abs(x - y) < 0.001f;
            }

            public int GetHashCode(float obj)
            {
                return Math.Round(obj, 3).GetHashCode();
            }
        }
    }
}
