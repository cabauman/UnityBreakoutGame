using NUnit.Framework;
using UnityEngine.TestTools.Constraints;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace GameCtor.DevToolbox
{
    public class EditModeTests
    {
        [Test]
        public void PlayModeTestsWithEnumeratorPasses()
        {
            var sut = new CoroutineUtil();
            Assert.That(() => { sut.WaitForSeconds(0.05f); }, Is.AllocatingGCMemory());
            Assert.That(() => { sut.WaitForSeconds(0.05f); }, Is.Not.AllocatingGCMemory());
            Assert.That(() => { sut.WaitForSeconds(0.06f); }, Is.AllocatingGCMemory());
            Assert.That(() => { sut.WaitForSeconds(0.06f); }, Is.Not.AllocatingGCMemory());
        }

        //[UnityTest]
        //public System.Collections.IEnumerator MyMethodShouldNotAllocateGC()
        //{
        //    yield return Measure.Method(() =>
        //    {
        //        // Call the method you want to test for GC allocations
        //        MyClass.MyMethod();
        //    })
        //    .DontAllocateGCMemory() // This is the key part for checking GC allocations
        //    .Run();
        //}
    }
}
