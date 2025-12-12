using NSubstitute;
using NUnit.Framework;
using System;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace GameCtor.DevToolbox
{
    public class EditModeTests
    {
        public class MyClass
        {
            public virtual event Action<int> MyEvent;
        }

        [Test]
        public void MockTest()
        {
            var mock = Substitute.For<MyClass>();
            int receivedValue = 0;
            mock.MyEvent += (value) => { receivedValue = value; };
            mock.MyEvent += Raise.Event<Action<int>>(42);
            Assert.That(receivedValue, Is.EqualTo(42));
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
