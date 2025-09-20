using BreakoutGame;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class NewTestScript
{
    [UnityTest]
    [LoadScene("Assets/Scenes/DummyScene.unity")]
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

public class LoadSceneAttribute : NUnitAttribute, IOuterUnityTestAction
{
    private readonly string _sceneName;

    public LoadSceneAttribute(string sceneName)
    {
        _sceneName = sceneName;
    }

    public IEnumerator BeforeTest(ITest test)
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode(_sceneName, new LoadSceneParameters(LoadSceneMode.Single));
    }

    public IEnumerator AfterTest(ITest test)
    {
        // unload
        //yield return new EnterPlayMode();
        yield return new ExitPlayMode();
    }
}
