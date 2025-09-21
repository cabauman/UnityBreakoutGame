using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UniDig;

namespace GameCtor.DevToolbox.Editor
{
    [CustomEditor(typeof(MonoInjectParamCustomizer))]
    public class MonoInjectParamCustomizerEditor : UnityEditor.Editor
    {
        private VisualElement _root;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            _root.Add(CreateContent());
            return _root;
        }

        private void OnEnable()
        {
            Undo.undoRedoEvent += OnUndoRedo;
        }

        private void OnDisable()
        {
            Undo.undoRedoEvent -= OnUndoRedo;
        }

        private VisualElement CreateContent()
        {
            var root = new VisualElement();

            var paramCustomizer = (MonoInjectParamCustomizer)target;
            var keysProperty = serializedObject.FindProperty("keys");

            if (!paramCustomizer.TryGetComponent<IMonoInject>(out var monoInject))
            {
                var helpBox = new HelpBox(
                    "This component is only valid on a GameObject with an IMonoInject component.",
                    HelpBoxMessageType.Warning);
                root.Add(helpBox);
                return root;
            }

            // NOTE: Could fail if the user implements an Inject method overload.
            var monoInjectType = monoInject.GetType();
            MethodInfo injectMethod = monoInjectType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == "Inject" && m.DeclaringType == monoInjectType);

            if (injectMethod == null)
            {
                return root;
            }

            ParameterInfo[] parameters = injectMethod.GetParameters();
            var keysCopy = new List<MonoInjectParam>(paramCustomizer.Keys);
            var actualParamNameHashSet = new HashSet<string>(keysCopy.Select(p => p.ParamName));

            foreach (var param in parameters)
            {
                if (!actualParamNameHashSet.Contains(param.Name))
                {
                    keysProperty.InsertArrayElementAtIndex(keysCopy.Count);
                    var key = keysProperty.GetArrayElementAtIndex(keysCopy.Count);
                    key.FindPropertyRelative("ParamName").stringValue = param.Name;
                    key.FindPropertyRelative("Key").objectReferenceValue = null;
                    keysCopy.Add(new MonoInjectParam { ParamName = param.Name });
                }
            }

            var indicesToRemove = new List<int>();
            var expectedParamNameHashSet = new HashSet<string>(parameters.Select(p => p.Name));
            for (int i = keysProperty.arraySize - 1; i >= 0; --i)
            {
                var pair = keysProperty.GetArrayElementAtIndex(i);
                var key = pair.FindPropertyRelative("Key").objectReferenceValue;
                var paramName = pair.FindPropertyRelative("ParamName").stringValue;

                // NOTE: Only remove a key if it is "truly" null. MissingReference is not truly null.
                // If it's a missing/existing reference, the user should review and delete it manually.
                if (!expectedParamNameHashSet.Contains(paramName) && ReferenceEquals(key, null))
                {
                    indicesToRemove.Add(i);
                    continue;
                }

                var keyField = new ObjectField(paramName);
                keyField.style.flexShrink = 1;
                keyField.objectType = typeof(StringVariable);
                var serializedKey = keysProperty.GetArrayElementAtIndex(i);
                keyField.value = keysCopy[i].Key;
                keyField.RegisterValueChangedCallback((evt) =>
                {
                    serializedKey.FindPropertyRelative("Key").objectReferenceValue = evt.newValue;
                    serializedObject.ApplyModifiedProperties();
                });

                VisualElement row = keyField;
                if (!expectedParamNameHashSet.Contains(paramName))
                {
                    row = new VisualElement();
                    keyField.SetEnabled(false);
                    row.style.flexDirection = FlexDirection.Row;
                    row.Add(keyField);
                    MonoInjectParam realKey = keysCopy[i];
                    var deleteButton = CreateButton("Toolbar Minus", () =>
                    {
                        var realIndex = keysCopy.IndexOf(realKey);
                        keysCopy.RemoveAt(realIndex);
                        keysProperty.DeleteArrayElementAtIndex(realIndex);
                        root.Remove(row);
                        serializedObject.ApplyModifiedProperties();
                    });
                    row.Add(deleteButton);
                }

                root.Add(row);
            }

            foreach (var index in indicesToRemove)
            {
                keysCopy.RemoveAt(index);
                keysProperty.DeleteArrayElementAtIndex(index);
            }

            serializedObject.ApplyModifiedProperties();

            return root;
        }

        private void OnUndoRedo(in UndoRedoInfo undo)
        {
            serializedObject.Update();
            _root.Clear();
            _root.Add(CreateContent());
        }

        private static Button CreateButton(string iconName, Action action)
        {
            var button = new Button(action);
            button.style.width = 24;
            var addIcon = EditorGUIUtility.IconContent(iconName).image as Texture2D;
            button.Add(new Image { image = addIcon });
            button.style.alignItems = Align.Center;
            button.style.justifyContent = Justify.Center;
            return button;
        }
    }
}
