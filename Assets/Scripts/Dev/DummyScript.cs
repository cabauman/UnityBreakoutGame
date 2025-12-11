using GameCtor.DevToolbox;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

#nullable enable

namespace BreakoutGame
{
    public class DummyScript : MonoBehaviour
    {
        void Start()
        {
            //var list = new System.Collections.Generic.List<int> { 1, 2, 3, 4, 5 };
            //var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list);
            //foreach (var item in span)
            //{
            //    Debug.Log(item);
            //}

            //ShouldNotAllocate();

            //int i = 24;
            float f = 3.14f;
            string s = null;
            //Span<char> chars = new char[] { 'a', 'b', 'c' };
            //ULog.Log($"Hello, {i}, {i+1}, {i+2}, {i+3}");
            //Debug.Log($"Hello, {i}, {i + 1}, {i + 2}, {i + 3}");
            //Test1();
            //Test2($"Hello, {i}, {i + 1}, {i + 2}, {i + 3}");
            //yield return null;
            //Test1();
            //Test2($"Hello, {f::F3}");
            ULog.Info($"{f:F3}, {s}");
        }

        public void Test1()
        {
            int i = 24;
            float f = 3.14f;
            var s = $"Hello, {i}, {i + 1}, {i + 2}, {i + 3}";
        }

        public void Test2(InterpolatedStringHandler handler)
        {
            string s = handler.ToString();
        }

        public void ShouldNotAllocate()
        {
            ReadOnlySpan<char> span = ",a,b,,c,d,";
            ReadOnlySpan<char> separator = ",";

            var recorder = Recorder.Get("GC.Alloc");

            // The recorder was created enabled, which means it captured the creation of the Recorder object itself, etc.
            // Disabling it flushes its data, so that we can retrieve the sample block count and have it correctly account
            // for these initial allocations.
            recorder.enabled = false;
            recorder.FilterToCurrentThread();
            recorder.enabled = true;

            try
            {
                foreach (var segment in span.Split(separator))
                {
                }
            }
            finally
            {
                recorder.enabled = false;
                recorder.CollectFromAllThreads();
            }

            Debug.Log($"recorder.sampleBlockCount: {recorder.sampleBlockCount}");
        }
    }
}
