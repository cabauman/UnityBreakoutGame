using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniDig;

namespace GameCtor.DevToolbox.Editor
{
    /// <summary>
    /// Searches all enabled scenes for MonoInject objects and assigns them to the scene's SceneBootstrapper.
    /// This is done to avoid the search cost at runtime.
    /// </summary>
    public sealed class SceneDiBuildPreprocessor : IPreprocessBuildWithReport
    {
        [InitializeOnLoadMethod]
        private static void PlayModeStateChangedExample()
        {
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
            EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
        }

        private static void HandlePlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                if (MonoInjectValidator.FindMissingMonoInjectParamKeyReferencesInScene(SceneManager.GetActiveScene()))
                {
                    // cancel entering play mode
                    EditorApplication.isPlaying = false;
                }
            }
        }

        [MenuItem("DevToolbox/AssignMonoInjectObjectsToSceneBootstrapper")]
        public static void AssignMonoInjectObjectsToSceneBootstrappers()
        {
            foreach (EditorBuildSettingsScene sceneConfig in EditorBuildSettings.scenes)
            {
                if (!sceneConfig.enabled)
                {
                    continue;
                }

                var scene = EditorSceneManager.GetSceneByPath(sceneConfig.path);
                if (!scene.isLoaded)
                {
                    scene = EditorSceneManager.OpenScene(sceneConfig.path, OpenSceneMode.Additive);
                    MonoInjectValidator.FindMissingMonoInjectParamKeyReferencesInScene(scene);
                    AssignMonoInjectObjectsToSceneBootstrapper(scene);
                    EditorSceneManager.CloseScene(scene, true);
                    continue;
                }

                MonoInjectValidator.FindMissingMonoInjectParamKeyReferencesInScene(scene);
                AssignMonoInjectObjectsToSceneBootstrapper(scene);
            }
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            AssignMonoInjectObjectsToSceneBootstrappers();
        }

        private static void AssignMonoInjectObjectsToSceneBootstrapper(Scene scene)
        {
            MonoBehaviour[] monoInjectObjects = scene.GetRootGameObjects()
                .SelectMany(x => x.GetComponentsInChildren<MonoBehaviour>(true))
                .Where(x => x is IMonoInject)
                .ToArray();

            //ULog.Debug($"{monoInjectObjects.Length} MonoInject objects found in scene '{scene.name}'");

            if (monoInjectObjects.Length > 0)
            {
                BaseSceneBootstrapper sceneBootstrapper = null;
                foreach (var go in scene.GetRootGameObjects())
                {
                    sceneBootstrapper = go.GetComponentInChildren<BaseSceneBootstrapper>();
                    if (sceneBootstrapper != null)
                    {
                        break;
                    }
                }

                if (sceneBootstrapper != null)
                {
                    sceneBootstrapper.monoInjectObjects = monoInjectObjects;
                    SceneBootstrapperGenerator.Generate(sceneBootstrapper);
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveScene(scene);
                }
                else
                {
                    //ULog.Warn($"Found MonoInject objects in scene, but no SceneBootstrapper was found: {scene.path}");
                }
            }
        }
    }
}
