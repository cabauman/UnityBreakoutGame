using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace GameCtor.DevToolbox
{
    public interface IPostInject
    {
        void PostInject() => Debug.Log("Default PostInject");
    }

    public static class StartupLifecycle
    {
        // Marker type
        private struct PostAwakeInjection { }
        private static Action _onInject;

        private struct PreStartInjection { }
        private static Action _onPostInject;

        public static void Initialize()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            InsertSystem<Initialization>(ref playerLoop, typeof(PostAwakeInjection), RunPostAwake);
            InsertSystem<Initialization>(ref playerLoop, typeof(PreStartInjection), RunPreStart);
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        public static void AddInjectListener(Action callback) => _onInject += callback;
        public static void AddPostInjectListener(Action callback) => _onPostInject += callback;

        private static void RunPostAwake()
        {
            _onInject?.Invoke();
            _onInject = null;
        }

        private static void RunPreStart()
        {
            _onPostInject?.Invoke();
            _onPostInject = null;
        }

        private static void InsertSystem<T>(
            ref PlayerLoopSystem root,
            Type systemType,
            PlayerLoopSystem.UpdateFunction updateFunc)
        {
            for (int i = 0; i < root.subSystemList.Length; i++)
            {
                if (root.subSystemList[i].type == typeof(T))
                {
                    var subs = root.subSystemList[i].subSystemList;
                    Array.Resize(ref subs, subs.Length + 1);

                    subs[^1] = new PlayerLoopSystem
                    {
                        type = systemType,
                        updateDelegate = updateFunc
                    };

                    root.subSystemList[i].subSystemList = subs;
                    return;
                }
            }
        }
    }
}
