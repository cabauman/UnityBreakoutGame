#if UNITY_EDITOR
using System.Collections;
using System.Linq;
//using Jab;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace GameCtor.DevToolbox
{
    public class DITests
    {
        //[UnitySetUp]
        public IEnumerator Setup()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(
                "Assets/Sandbox/Colt/DraftScene.unity",
                new LoadSceneParameters { loadSceneMode = LoadSceneMode.Single });
        }

        //[UnityTest]
        //public IEnumerator AllCompositionRootsInSceneResolveWithoutError()
        //{
        //    yield return null;
        //    //var scene = SceneManager.GetActiveScene();
        //    var compositionRoots = GameObject.FindObjectsOfType<BaseCompositionRoot>(true);
        //    Assert.AreEqual(2, compositionRoots.Length);

        //    var serviceProviderType = typeof(IServiceProvider<>);
        //    var getServiceMethodBase = typeof(BaseCompositionRoot).GetMethod("GetService");
        //    foreach (var compositionRoot in compositionRoots)
        //    {
        //        var providedTypes = compositionRoot.GetType()
        //            .GetInterfaces()
        //            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == serviceProviderType)
        //            .Select(x => x.GetGenericArguments()[0]);
        //        foreach (var providedType in providedTypes)
        //        {
        //            Debug.Log($"Constructing {providedType.FullName}");
        //            var getServiceMethod = getServiceMethodBase.MakeGenericMethod(providedType);
        //            var result = getServiceMethod.Invoke(compositionRoot, new object[] { null });
        //            Assert.IsNotNull(result);
        //        }
        //    }
        //}
    }

        //    // Simulate a mouse click event at the center of the screen
        //    //Press(mouse.leftButton);
        //    //Set(mouse.position, new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        //    Vector2 clickPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        //    InputSystem.QueueStateEvent(mouse, new MouseState() { position = clickPosition }.WithButton(MouseButton.Left, true));

        //    // Wait for one frame
        //    yield return null;
}
#endif
