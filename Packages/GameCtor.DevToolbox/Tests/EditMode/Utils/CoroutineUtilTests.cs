using NUnit.Framework;
using UnityEngine.TestTools.Constraints;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace GameCtor.DevToolbox
{
    public class CoroutineUtilTests
    {
        [TestCase(1.0f, 1.0f)]
        [TestCase(0.1f, 0.1f)]
        [TestCase(0.02f, 0.02f)]
        [TestCase(0.003f, 0.003f)]
        [TestCase(0.004f, 0.0041f)]
        public void ShouldNotAllocateAfterFirstAccessPerValue(float a, float b)
        {
            var sut = new CoroutineUtil();
            Assert.That(() => { sut.WaitForSeconds(a); }, Is.AllocatingGCMemory());
            Assert.That(() => { sut.WaitForSeconds(b); }, Is.Not.AllocatingGCMemory());
        }

        [TestCase(1.0f, 1.1f)]
        [TestCase(0.1f, 0.2f)]
        [TestCase(0.02f, 0.03f)]
        [TestCase(0.003f, 0.004f)]
        public void ShouldAllocateAfterFirstAccessPerValue(float a, float b)
        {
            var sut = new CoroutineUtil();
            Assert.That(() => { sut.WaitForSeconds(a); }, Is.AllocatingGCMemory());
            Assert.That(() => { sut.WaitForSeconds(b); }, Is.AllocatingGCMemory());
        }
    }
}
