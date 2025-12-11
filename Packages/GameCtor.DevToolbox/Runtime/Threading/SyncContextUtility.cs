using System.Threading;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    /// <summary>
    /// Utility class to assist in thread and context synchronization.
    /// </summary>
    public static class SyncContextUtility
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            UnitySynchronizationContext = SynchronizationContext.Current;
            UnityThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// Gets the Unity main thread ID.
        /// </summary>
        public static int UnityThreadId { get; private set; }

        /// <summary>
        /// Gets the Unity main thread synchronization context.
        /// </summary>
        public static SynchronizationContext UnitySynchronizationContext { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current thread is the Unity main thread.
        /// </summary>
        public static bool IsMainThread => UnitySynchronizationContext == SynchronizationContext.Current;
    }
}
