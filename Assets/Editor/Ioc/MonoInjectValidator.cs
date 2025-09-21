using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniDig;

namespace GameCtor.DevToolbox.Editor
{
    public sealed class MonoInjectValidator
    {
        public static bool FindMissingMonoInjectParamKeyReferencesInScene(Scene scene)
        {
            var paramCustomizers = scene.GetRootGameObjects()
                .SelectMany(x => x.GetComponentsInChildren<MonoInjectParamCustomizer>(true))
                .ToArray();

            var foundOutdatedParams = false;
            foreach (var paramCustomizer in paramCustomizers)
            {
                if (!paramCustomizer.TryGetComponent<IMonoInject>(out var monoInject))
                {
                    continue;
                }

                var monoInjectType = monoInject.GetType();
                MethodInfo injectMethod = monoInjectType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name == "Inject" && m.DeclaringType == monoInjectType);

                if (injectMethod == null)
                {
                    continue;
                }

                ParameterInfo[] parameters = injectMethod.GetParameters();
                var expectedParamNameHashSet = new HashSet<string>(parameters.Select(p => p.Name));

                var outdatedParams = new List<MonoInjectParam>();
                for (int i = 0; i < paramCustomizer.Keys.Count; i++)
                {
                    var paramInfo = paramCustomizer.Keys[i];
                    var paramName = paramInfo.ParamName;
                    // Only report an error if the param name no longer exists AND the key is non-null (missing/existing).
                    // If the key is null, it means that param doesn't require named DI registration.
                    if (!expectedParamNameHashSet.Contains(paramName) && !ReferenceEquals(paramInfo.Key, null))
                    {
                        var soPath = AssetDatabase.GetAssetPath(paramInfo.Key);
                        Debug.Log(soPath);
                        outdatedParams.Add(paramInfo);
                    }
                }

                if (outdatedParams.Count > 0)
                {
                    foundOutdatedParams = true;
                    var outdatedParamStrings = outdatedParams.Select(x => $"param: {x.ParamName}, key: {x.Key.Value}");
                    var outdatedParamString = string.Join(" | ", outdatedParamStrings);
                    var actualParams = string.Join(", ", parameters.Select(x => x.Name));
                    var pathToObject = GetTransformPath(paramCustomizer.transform);
                    //ULog.Error($"Outdated {nameof(MonoInjectParamCustomizer)} reference.\nOutdated params: " +
                    //    $"{outdatedParamString}\nActual params: {actualParams}\nPath: {pathToObject}", paramCustomizer);
                }
            }

            if (foundOutdatedParams)
            {
                var message = $"Found outdated {nameof(MonoInjectParamCustomizer)} references in scene '{scene.name}'. " +
                    "This means that each outdated component contains a custom DI key was assigned to a parameter, but that " +
                    "parameter name no longer exists, due to removing or renaming dependencies. This can be resolved " +
                    "by reviewing the outdated keys in the scene and removing them.";
                //throw new Exception(message);
                //ULog.Error(message);
            }

            return foundOutdatedParams;
        }

        private static string GetTransformPath(Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = $"{transform.name}/{path}";
            }
            return path;
        }
    }
}
