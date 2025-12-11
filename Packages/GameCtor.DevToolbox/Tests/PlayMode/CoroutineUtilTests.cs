using GameCtor.DevToolbox;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Constraints;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace GameCtor.DevToolbox
{
    public class CoroutineUtilTests
    {
        [UnityTest]
        public IEnumerator Test1()
        {
            var sut = new CoroutineUtil();
            var start = Time.realtimeSinceStartup;
            yield return sut.WaitForSecondsRealtime(0.5f);
            var delta = Time.realtimeSinceStartup - start;
            Assert.That(delta, Is.EqualTo(0.5f).Within(0.1f));

            start = Time.realtimeSinceStartup;
            yield return sut.WaitForSecondsRealtime(0.5f);
            delta = Time.realtimeSinceStartup - start;
            Assert.That(delta, Is.EqualTo(0.5f).Within(0.1f));
        }

        [UnityTest]
        public IEnumerator WaitForSecondsRealtimeTest()
        {
            var sut = new CoroutineUtil();
            var a = new GameObject("Dummy").AddComponent<DummyClass>();
            a.StartCoroutine(TestCoroutine(sut));
            yield return new WaitForSeconds(1.1f);
            a.StartCoroutine(TestCoroutine(sut));
            yield return new WaitForSeconds(2f);
            Object.Destroy(a.gameObject);
        }

        [UnityTest]
        public IEnumerator WaitForSecondsRealtimeTest2()
        {
            var sut = new CoroutineUtil();
            var a = new GameObject("Dummy").AddComponent<DummyClass>();
            a.StartCoroutine(TestCoroutine(sut));
            yield return new WaitForSeconds(0.5f);
            a.StartCoroutine(TestCoroutine(sut));
            yield return new WaitForSeconds(2f);
            Object.Destroy(a.gameObject);
        }

        private IEnumerator TestCoroutine(CoroutineUtil sut)
        {
            var start = Time.realtimeSinceStartup;
            var wfs = sut.WaitForSecondsRealtime(1f);
            yield return wfs;
            sut.ReturnWaitForSecondsRealtime(wfs);
            var delta = Time.realtimeSinceStartup - start;
            Assert.That(delta, Is.EqualTo(1f).Within(0.1f));
        }

        [UnityTest]
        public IEnumerator WaitForSecondsTest()
        {
            var sut = new CoroutineUtil();
            var a = new GameObject("Dummy").AddComponent<DummyClass>();
            a.StartCoroutine(TestCoroutine2(sut));
            yield return new WaitForSeconds(0.5f);
            a.StartCoroutine(TestCoroutine2(sut));
            yield return new WaitForSeconds(2f);
            Object.Destroy(a.gameObject);
        }

        private IEnumerator TestCoroutine2(CoroutineUtil sut)
        {
            var start = Time.realtimeSinceStartup;
            yield return sut.WaitForSeconds(1f);
            var delta = Time.realtimeSinceStartup - start;
            Assert.That(delta, Is.EqualTo(1f).Within(0.1f));
        }

        public class DummyClass : MonoBehaviour
        {
        }
    }
}
