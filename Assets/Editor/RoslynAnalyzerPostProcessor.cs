//using System.IO;
//using System.Linq;
//using System.Xml.Linq;
//using UnityEditor;
//using UnityEngine;

//namespace GameCtor.DevToolbox.Editor
//{
//    public class RoslynAnalyzerPostProcessor : AssetPostprocessor
//    {
//        public static string OnGeneratedCSProject(string path, string content)
//        {
//            var document = XDocument.Parse(content);
//            if (document.Root == null)
//            {
//                return content;
//            }

//            XNamespace xNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
//            var analyzerItemGroup = document
//                .Element(xNamespace + "Project")
//                .Elements(xNamespace + "ItemGroup")
//                .FirstOrDefault(x => x.Elements(xNamespace + "Analyzer").Any());

//            if (analyzerItemGroup is null)
//            {
//                return content;
//            }

//            var analyzerElements = analyzerItemGroup
//                .Elements(xNamespace + "Analyzer")
//                .ToList();

//            XName analyzerAttributeName = "Include";

//            // Remove the default Unity analyzer dll because we can't control the severity level that appears
//            // in the Unity console.
//            // Instead, we use a copy of the Unity analyzers dll (with ".Custom" appended to the name) which
//            // we added to the Unity project explicitly.
//            analyzerElements.RemoveAll(x =>
//                DllEndsWith(x, "Microsoft.Unity.Analyzers.dll") &&
//                !DllIsInProject(x));

//            analyzerItemGroup.ReplaceAll(analyzerElements);

//            return document.ToString();
//        }

//        private static bool DllEndsWith(XElement x, string dllName)
//        {
//            XName analyzerAttributeName = "Include";
//            return x.Attribute(analyzerAttributeName).Value.EndsWith(dllName);
//        }

//        private static bool DllIsInProject(XElement x)
//        {
//            XName analyzerAttributeName = "Include";
//            var dllPath = x.Attribute(analyzerAttributeName).Value;
//            dllPath = Path.GetFullPath(dllPath);
//            return dllPath.StartsWith(Path.GetFullPath(Application.dataPath));
//        }
//    }
//}
