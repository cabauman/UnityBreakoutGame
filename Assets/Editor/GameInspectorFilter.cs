using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[CustomEditor(typeof(GameObject))]
public class GameObjectInspectorFilter : Editor
{
    //private static int activeFilter = -1; // -1 = show all, otherwise index of component

    //public override void OnInspectorGUI()
    //{
    //    //base.OnInspectorGUI();
    //    //var go = (GameObject)target;
    //    //var comps = go.GetComponents<Component>()

    //    //foreach (var comp in comps)
    //    //{
    //    //    Editor editor = Editor.CreateEditor(comp);
    //    //    IMGUIContainer inspectorIMGUI = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
    //    //}
    //}

    //public override VisualElement CreateInspectorGUI()
    //{
    //    var root = new VisualElement();
    //    //var go = (GameObject)target;
    //    //var comps = go.GetComponents<Component>();


    //    //foreach (var comp in comps)
    //    //{
    //    //    Editor editor = Editor.CreateEditor(comp);
    //    //    IMGUIContainer inspectorIMGUI = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
    //    //    root.Add(inspectorIMGUI);
    //    //}

    //    return root;
    //}
}
