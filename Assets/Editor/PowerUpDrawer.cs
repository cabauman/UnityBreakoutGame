using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace BreakoutGame
{
    //[CustomPropertyDrawer(typeof(PowerUpConfig), useForChildren: true)]
    public class PowerUpDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var field = new PropertyField(property);
            container.Add(field);

            if (property.managedReferenceValue == null)
            {
                property.managedReferenceValue = Activator.CreateInstance(typeof(PowerUpConfig));
                Debug.Log("managedReferenceValue");
                property.serializedObject.ApplyModifiedProperties();
            }

            return container;
        }
    }
}