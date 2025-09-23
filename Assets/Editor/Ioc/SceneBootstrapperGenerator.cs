using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace GameCtor.DevToolbox.Editor
{
    public static class SceneBootstrapperGenerator
    {
        public static void Generate(BaseSceneBootstrapper bootstrapper)
        {
            var bootstrapperType = bootstrapper.GetType();
            if (bootstrapperType == typeof(BaseSceneBootstrapper))
            {
                //ULog.Warn($"BaseSceneBootstrapper is not a valid type for code generation. Use a derived class " +
                //    "if code generation is desired.");
                return;
            }

            var monoInjectObjects = bootstrapper.monoInjectObjects;
            var monoInjectObjectsByType = monoInjectObjects.GroupBy(x => x.GetType());
            var methodBodyList = new List<(ParameterInfo[] Parameters, string FullTypeName, bool hasKeys)>();
            foreach (var monoInject in monoInjectObjectsByType)
            {
                var monoType = monoInject.First().GetType();
                var hasKeys = monoInject.Any(x => x.TryGetComponent<MonoInjectParamCustomizer>(out var _));
                var injectMethod = GetInjectMethod(monoType);
                if (injectMethod == null)
                {
                    //ULog.Error($"No Inject method found for {monoType.FullName}");
                    continue;
                }
                var parameters = injectMethod.GetParameters();
                methodBodyList.Add((parameters, monoType.FullName, hasKeys));
            }

            var namespaceName = bootstrapperType.Namespace;

            var writer = new SourceWriter();
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using System.Linq;");
            writer.AppendLine("using DevToolbox;");
            writer.AppendLine("using GameCtor.ULogging;");
            writer.AppendLine("using UnityEngine;").AppendLine();

            using (writer.Namespace(namespaceName))
            {
                using (writer.BlockFormat("partial class {0}", bootstrapperType.Name))
                {
                    writer.AppendLine("#if !UNITY_EDITOR");
                    using (writer.BlockFormat("protected override void InjectSceneDependencies(MonoBehaviour[] behaviours)"))
                    {
                        writer.AppendLine("ULog.Debug(\"Injecting dependencies from generated source code.\");");
                        writer.AppendLine("var resolving = new HashSet<TypeKey>();");
                        using (writer.Block("foreach (var behaviour in behaviours)"))
                        {
                            using (writer.Block("switch (behaviour.GetType().FullName)"))
                            {
                                foreach (var (Parameters, FullTypeName, hasKeys) in methodBodyList)
                                {
                                    using var @case = writer.BlockFormatWithoutBraces($"case \"{FullTypeName}\":");
                                    writer.AppendLineFormat("Resolve{0}(behaviour, resolving);", FullTypeName.Substring(FullTypeName.IndexOf(".") + 1));
                                    writer.AppendLine("break;");
                                }
                            }
                        }
                    }

                    foreach (var (Parameters, FullTypeName, hasKeys) in methodBodyList)
                    {
                        using (writer.BlockFormat("private void Resolve{0}(MonoBehaviour behaviour, HashSet<TypeKey> resolving)", FullTypeName.Substring(FullTypeName.IndexOf(".") + 1)))
                        {
                            writer.AppendLine($"var monoInject = behaviour as {FullTypeName};");
                            if (hasKeys)
                            {
                                writer.AppendLine("Dictionary<string, string> keys = null;");
                                using (writer.Block("if (behaviour.TryGetComponent<MonoInjectParamCustomizer>(out var paramCustomizer))"))
                                {
                                    writer.AppendLine("keys = paramCustomizer.Keys.Where(x => x.Key != null).ToDictionary(x => x.ParamName, x => x.Key.Value);");
                                }
                            }
                            writer.AppendLine("string key = null;");
                            WriteInjectString(Parameters, hasKeys, writer);
                        }
                    }
                    writer.AppendLine("#endif");
                }
            }

            var assetPath = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(bootstrapper));
            var directory = Path.GetDirectoryName(assetPath);
            var path = $"{directory}/{bootstrapperType.Name}.g.cs";
            //var path = $"../{bootstrapperType.Name}.g.cs";
            File.WriteAllText(path, writer.ToString());
            AssetDatabase.Refresh();
        }

        private static void WriteInjectString(ParameterInfo[] parameters, bool hasKeys, SourceWriter writer)
        {
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (hasKeys)
                {
                    writer.AppendLine($"keys?.TryGetValue(\"{parameters[i].Name}\", out key);");
                }
                writer.AppendLine("resolving.Clear();");
                writer.AppendLine("resolving.Add(new TypeKey(behaviour.GetType(), key));");
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
