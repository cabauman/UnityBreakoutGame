using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UniDig;
using UnityEditor;
using UnityEngine;

namespace GameCtor.DevToolbox.Editor
{
    public static class FactoryGenerator
    {
        [MenuItem("DevToolbox/GeneratePrefabFactory")]
        public static void Generate()
        {
            var targetAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "BreakoutGame");

            Type type = null;
            if (targetAssembly != null)
            {
                type = targetAssembly.GetType("UniDig.IMonoInject");
            }
            var monoInjectTypes = TypeCache.GetTypesDerivedFrom(type);
            //var interfaceType = typeof(IMonoInject);
            //var monoInjectTypes = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(a => a.GetTypes())
            //    .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            //    .ToList();

            var methodBodyList = new List<(ParameterInfo[] Parameters, string FullTypeName)>();
            foreach (var monoInjectType in monoInjectTypes)
            {
                var injectMethod = GetInjectMethod(monoInjectType);
                if (injectMethod == null)
                {
                    Debug.LogError($"No Inject method found for {monoInjectType.FullName}");
                    continue;
                }
                var parameters = injectMethod.GetParameters();
                methodBodyList.Add((parameters, monoInjectType.FullName));
            }

            var namespaceName = "GameCtor.DevToolbox";

            var writer = new SourceWriter();
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using UnityEngine;").AppendLine();

            using (writer.Namespace(namespaceName))
            {
                using (writer.Block("public class PrefabFactory1"))
                {
                    writer.AppendLine("private BaseCompositionRoot _compositionRoot;").AppendLine();
                    writer.AppendLine("#if !UNITY_EDITOR");
                    foreach (var (Parameters, FullTypeName) in methodBodyList)
                    {
                        using (writer.BlockFormat("public void Visit({0} monoInject)", FullTypeName))
                        {
                            WriteInjectString(Parameters, writer);
                        }
                    }
                    writer.AppendLine("#endif");
                }
            }

            var path = "Assets/Scripts/PrefabFactory1.g.cs";
            File.WriteAllText(path, writer.ToString());
            AssetDatabase.Refresh();
        }

        private static void WriteInjectString(ParameterInfo[] parameters, SourceWriter writer)
        {
            writer.AppendLine("string key = null;");
            for (int i = 0; i < parameters.Length; ++i)
            {
                writer.AppendLine($"var arg{i} = _compositionRoot.Resolve<{parameters[i].ParameterType.FullName}>(key);");
            }

            var argString = string.Join(", ", parameters.Select((x, i) => $"arg{i}"));
            writer.AppendLine($"monoInject.Inject({argString});");
        }

        private static MethodInfo GetInjectMethod(Type monoType)
        {
            var injectMethods = monoType.GetMethods().Where(x => x.Name == "Inject").ToArray();
            if (injectMethods.Length == 0)
            {
                return null;
            }

            var currentType = monoType;
            MethodInfo injectMethod = null;
            while (injectMethod is null)
            {
                injectMethod = injectMethods.FirstOrDefault(x => x.DeclaringType == currentType);
                currentType = currentType.BaseType;
            }

            return injectMethod;
        }
    }
}
