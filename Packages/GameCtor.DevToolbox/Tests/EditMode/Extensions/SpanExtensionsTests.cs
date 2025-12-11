using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace GameCtor.DevToolbox
{
    public class SpanExtensionsTests
    {
        [Test]
        public void Test1()
        {
            ReadOnlySpan<char> span = ",a,b,,c,d,";
            ReadOnlySpan<char> separator = ",";

            var segments = new List<string>();
            foreach (var segment in span.Split(separator))
            {
                segments.Add(segment.ToString());
            }

            var expected = new List<string> { "a", "b", "c", "d" };
            Assert.AreEqual(expected, segments);
        }

        [Test]
        public void ShouldNotAllocate()
        {
            ReadOnlySpan<char> span = ",a,b,,c,d,";
            ReadOnlySpan<char> separator = ",";

            // JIT
            span.Split(separator);

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

            Assert.That(recorder.sampleBlockCount, Is.EqualTo(0), "Expected no GC allocations");
        }
    }
}
