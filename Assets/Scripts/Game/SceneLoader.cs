using GameCtor.DevToolbox;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreakoutGame
{
    public sealed class SceneLoader : MonoBehaviour
    {
        public void LoadGameScene(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            // Fade to black
            //yield return FadeCanvas.Instance.FadeOut();

            // Load scene async
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
            loadOp.allowSceneActivation = false;

            while (loadOp.progress < 0.9f)
            {
                yield return null;
            }

            ULog.Trace($"Scene '{sceneName}' almost loaded.");

            // Wait one frame to ensure scene is active
            yield return null;

            ULog.Trace($"Scene '{sceneName}' loaded.");

            // Activate scene
            loadOp.allowSceneActivation = true;

            ULog.Trace($"Scene '{sceneName}' activated.");

            // Fade back in
            //yield return FadeCanvas.Instance.FadeIn();
        }
    }
}