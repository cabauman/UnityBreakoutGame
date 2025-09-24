using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace BreakoutGame
{
    public class DummyTests : InputTestFixture
    {
        [UnityTest]
        public IEnumerator UnityTest()
        {
            Debug.Log("UnityTest before yield");
            yield return new EnterPlayMode();
            new GameObject().AddComponent<TestMono>();
            //Domain reload happening
            yield return new WaitForSeconds(1f);
            yield return new ExitPlayMode();
            Debug.Log("UnityTest after yield");
        }

        [Test]
        public void FloatValuesShouldBeEqual()
        {
            var hashset = new HashSet<float>(new FloatComparer());
            //float a = 0.112f;
            //float b = 0.113f;
            var a = Mathf.Epsilon;
            var b = Mathf.Epsilon / 2f;
            Debug.Log(Math.Round(double.MaxValue, 4));
            hashset.Add(a);
            Assert.That(hashset.Contains(b), Is.True);
            //Assert.That(a, Is.EqualTo(b).Using(new FloatComparer()));
        }

        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            var sut = new GameObject().AddComponent<TestMono>();
            //sut.DoSomething();
            //yield return null;
            yield return new WaitForSeconds(1f);
        }

        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses2()
        {
            var o = Observable
                .EveryUpdate()
                .Subscribe(x => Debug.Log(x));

            yield return null;
            yield return null;

            o.Dispose();

            //var sut = new GameObject().AddComponent<PlayingField>();
            //yield return null;
            //yield return null;
            //yield return null;
            //sut.DoSomething();
        }
    }

    class FloatComparer : IEqualityComparer<float>
    {
        public bool Equals(float x, float y) => Mathf.Abs(x - y) <= 0.0001f;
        public int GetHashCode(float obj) => obj.GetHashCode();
        //public int GetHashCode(float obj)
        //{
        //    return Math.Round(obj, 4).GetHashCode();
        //}
    }
}



// Either yield return null or call InputSystem.Update() to update the state of the input system.


// InputSystem.QueueStateEvent(mouse, new MouseState() { position = Vector2.zero }.WithButton(MouseButton.Right, true));
// yield return null;
// InputSystem.QueueDeltaStateEvent(mouse.position, new Vector2(100f, 0f));
// yield return null;
// InputSystem.QueueStateEvent(mouse, new MouseState() { position = Vector2.right * 100f }.WithButton(MouseButton.Right, false));
// yield return new WaitForSeconds(3f);