using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TestTools.Constraints;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace GameCtor.DevToolbox
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void TryGetNonEnumeratedCountTest()
        {
            // JIT
            Enumerable.Empty<int>().TryGetNonEnumeratedCount(out _);

            IEnumerable<int> enumerable = new List<int> { 1, 2, 3, 4, 5 };
            bool result = false;
            int count = 0;

            Assert.That(
            () =>
            {
                result = enumerable.TryGetNonEnumeratedCount(out count);
            },
            Is.Not.AllocatingGCMemory());

            Assert.IsTrue(result);
            Assert.AreEqual(5, count);
        }

        [Test]
        public void TryGetNonEnumeratedCountTest2()
        {
            // JIT
            Enumerable.Empty<int>().TryGetNonEnumeratedCount(out _);

            IEnumerable<int> enumerable = new HashSet<int> { 1, 2, 3, 4, 5 };
            bool result = false;
            int count = 0;

            Assert.That(
            () =>
            {
                result = enumerable.TryGetNonEnumeratedCount(out count);
            },
            Is.Not.AllocatingGCMemory());

            Assert.IsTrue(result);
            Assert.AreEqual(5, count);
        }

        [Test]
        public void TryGetNonEnumeratedCountTest3()
        {
            // JIT
            Enumerable.Empty<int>().TryGetNonEnumeratedCount(out _);

            var enumerable = Enumerable.Range(1, 5).Where(x => x % 2 == 0);
            bool result = false;
            int count = 0;

            Assert.That(
            () =>
            {
                result = enumerable.TryGetNonEnumeratedCount(out count);
            },
            Is.Not.AllocatingGCMemory());

            Assert.IsFalse(result);
            Assert.AreEqual(0, count);
        }
    }
}
