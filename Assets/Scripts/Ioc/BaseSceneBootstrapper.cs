using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameCtor.DevToolbox
{
    [DefaultExecutionOrder(-4999)]
    public class BaseSceneBootstrapper : MonoBehaviour
    {
        [SerializeField] protected BaseCompositionRoot _compositionRoot;
        [HideInInspector] public MonoBehaviour[] monoInjectObjects;

        /// <summary>
        /// Finds all <see cref="IMonoInject"/> objects in the scene and injects their dependencies.
        /// </summary>
        /// <remarks>
        /// Builds: <see cref="monoInjectObjects" /> is populated by a build preprocessor.
        /// </remarks>
        private void Awake()
        {
            StartupLifecycle.Initialize();
#if UNITY_EDITOR
            monoInjectObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(x => x is UniDig.IMonoInject)
                .ToArray();
#endif
            //InjectSceneDependencies(monoInjectObjects);
            StartupLifecycle.AddInjectListener(() => InjectSceneDependencies(monoInjectObjects));
            //foreach (var obj in monoInjectObjects)
            //{
            //    if (obj is UniDig.IMonoInject)
            //    {
            //        var methodInfo = obj.GetType().GetMethod("PostInit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //        if (methodInfo is not null)
            //        {
            //            StartupLifecycle.AddListener(() => methodInfo.Invoke(obj, null));
            //        }
            //    }
            //}
        }

        protected virtual void InjectSceneDependencies(MonoBehaviour[] monos)
        {
            var getServiceMethodBase = typeof(BaseCompositionRoot).GetMethod("Resolve");
            var resolving = new HashSet<TypeKey>();
            foreach (var m in monos)
            {
                var monoType = m.GetType();
                IDictionary<string, string> keys = null;
                if (m.TryGetComponent(out MonoInjectParamCustomizer paramCustomizer))
                {
                    keys = paramCustomizer.Keys.Where(x => x.Key != null).ToDictionary(x => x.ParamName, x => x.Key.Value);
                }

                var injectMethods = monoType.GetMethods().Where(x => x.Name == "Inject").ToArray();
                if (injectMethods.Length == 0)
                {
                    //ULog.Error($"No Inject method found for {monoType.FullName}");
                    UnityEngine.Debug.LogError($"No Inject method found for {monoType.FullName}");
                    continue;
                }
                var currentType = monoType;
                MethodInfo injectMethod = null;
                while (injectMethod is null)
                {
                    injectMethod = injectMethods.FirstOrDefault(x => x.DeclaringType == currentType);
                    currentType = currentType.BaseType;
                }

                var parameters = injectMethod.GetParameters();
                var injectArgs = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var parameterType = parameter.ParameterType;
                    var getServiceMethod = getServiceMethodBase.MakeGenericMethod(parameterType);
                    string key = null;
                    keys?.TryGetValue(parameter.Name, out key);
                    resolving.Clear();
                    resolving.Add(new TypeKey(monoType, key));
                    try
                    {
                        injectArgs[i] = getServiceMethod.Invoke(_compositionRoot, new object[] { key });
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException is null)
                        {
                            //ULog.Error($"Failed to resolve service '{parameterType.FullName}': {ex.Message}");
                            UnityEngine.Debug.LogError($"Failed to resolve service '{parameterType.FullName}': {ex.Message}");
                        }
                        else
                        {
                            var length = ex.InnerException.StackTrace.IndexOf("  at DevToolbox.BaseCompositionRoot.GetServiceShallow");
                            if (length == -1)
                            {
                                length = ex.InnerException.StackTrace.Length;
                            }
                            var stackTrace = ex.InnerException.StackTrace[..length];
                            var errorDetails = $"message: {ex.InnerException?.Message}\n<b>StackTrace</b>\n{stackTrace}================\n";
                            UnityEngine.Debug.LogError(errorDetails);
                            //ULog.Error($"Failed to resolve service '{parameterType.FullName}': {errorDetails}");
                        }
                    }
                }
                injectMethod.Invoke(m, injectArgs);
            }
        }
    }
}
