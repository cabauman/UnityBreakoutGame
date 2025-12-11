using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.TestTools.Constraints;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace GameCtor.DevToolbox
{
    public class CollectionsMarshalTests
    {
        [Test]
        public void AsSpanTest()
        {
            // JIT
            CollectionsMarshal.AsSpan(new List<int>());

            var list = new List<int> { 1, 2, 3, 4, 5 };
            int sum = 0;

            Assert.That(
            () =>
            {
                Span<int> span = CollectionsMarshal.AsSpan(list);
                foreach (var item in span)
                {
                    sum += item;
                }
            },
            Is.Not.AllocatingGCMemory());

            Assert.AreEqual(15, sum);
        }
    }
}
