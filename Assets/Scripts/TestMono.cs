﻿using System;
using System.Collections;
using Unity.Profiling;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;

namespace BreakoutGame
{
    public class TestMono : MonoBehaviour
    {
        readonly FrameTiming[] m_FrameTimings = new FrameTiming[1];
        private ProfilerRecorder gcAllocRecorder;
        private bool didRecordLastFrame = false;

        [SerializeReference]
        public BaseData[] data;

        private void Awake()
        {
            Debug.Log("Awake");
        }

        private void Start()
        {
            Debug.Log("Start");
        }

        void OnEnable()
        {
            // GC Allocation In Frame Count
            // GC Used Memory
            // GC Allocated In Frame
            gcAllocRecorder = ProfilerRecorder.StartNew(
                ProfilerCategory.Memory,
                "GC Allocated In Frame",
                1);
            Profiler.enabled = true;
        }

        void OnDisable()
        {
            gcAllocRecorder.Dispose();
        }

        /// <summary>
        /// Runs the given action and logs its GC allocation in bytes.
        /// </summary>
        public void MeasureAlloc(string label, System.Action action)
        {
            //gcAllocRecorder.Reset();

            action();

            long bytes = gcAllocRecorder.LastValue;
            Debug.Log($"{label} allocated {bytes} bytes GC");
        }
        public void MeasureAlloc(string label, System.Func<object> action)
        {
            action();
            didRecordLastFrame = true;

            long bytes = gcAllocRecorder.CurrentValue;
            gcAllocRecorder.Reset();
            Debug.Log($"{label} allocated {bytes} bytes GC");
        }

        void Update()
        {
            //if (UnityEngine.InputSystem.Keyboard.current == null)
            //{
            //    return;
            //}
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                //MeasureAlloc("MyMethod", MyMethod0);
                gameObject.AddComponent<TestMono>();
                Debug.Log("Added TestMono");
            }
            //if (didRecordLastFrame)
            //{
            //    long bytes = gcAllocRecorder.CurrentValue;
            //    Debug.Log($"MyMethod allocated {bytes} bytes GC");
            //    didRecordLastFrame = false;
            //}
        }

        public static void DumpGCAllocForFrame(int frameIndex)
        {
            //int threadCount = ProfilerDriver.GetThreadCount(frameIndex);
            //for (int t = 0; t < threadCount; t++)
            //{
            //    var threadName = ProfilerDriver.GetThreadName(frameIndex, t, out var groupName);
            //    Debug.Log($"Thread: {groupName}/{threadName}");
            //    // you'd need reflection into ProfilerFrameData* classes here
            //}
        }

        private void CaptureTimings()
        {
            FrameTimingManager.CaptureFrameTimings();
            FrameTimingManager.GetLatestTimings(1, m_FrameTimings);
        }

        object MyMethod0()
        {
            return new object();
        }

        void MyMethod()
        {
            // Example allocation
            var arr = new int[1000];
            var s = new string('a', 200);
        }
    }

    [Serializable]
    public abstract class BaseData
    {
        public int MyInt;
    }
    [Serializable]
    public class DerivedDataA : BaseData
    {
        public string MyStringA;
        public float MyFloatA;
    }
    [Serializable]
    public class DerivedDataB : BaseData
    {
        public bool MyBool;
    }
}