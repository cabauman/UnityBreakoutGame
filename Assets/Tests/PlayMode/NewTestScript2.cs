using BreakoutGame;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript2
{
    [UnityTest]
    public IEnumerator Test1()
    {
        var sut = new GameObject().AddComponent<TestMono>();
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator Test2()
    {
        var sut = new GameObject().AddComponent<TestMono>();
        yield return new WaitForSeconds(1f);
    }

    [UnityTest] public IEnumerator Test3()
    {
        var sut = new GameObject().AddComponent<TestMono>();
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator Test4()
    {
        var sut = new GameObject().AddComponent<TestMono>();
        yield return new WaitForSeconds(1f);
    }
}
