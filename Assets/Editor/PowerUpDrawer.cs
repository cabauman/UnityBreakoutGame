using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(PowerUpConfig), true)]
public class PowerUpDrawer : PropertyDrawer
{
    static Dictionary<string, Type> typeMap;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        if (typeMap == null) BuildTypeMap();

        var container = new VisualElement();

        var typeName = property.managedReferenceFullTypename;
        var displayName = GetShortTypeName(typeName);
        var options = new List<string> { "Select Effect Type" };
        options.AddRange(typeMap.Keys);

        var dropdown = new DropdownField("Effect Type", options, displayName ?? "Select Effect Type");
        dropdown.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue == "Select Effect Type")
            {
                property.managedReferenceValue = null;
            }
            else
            {
                var selectedType = typeMap[evt.newValue];
                property.managedReferenceValue = Activator.CreateInstance(selectedType);
            }
            property.serializedObject.ApplyModifiedProperties();
            // Force UI update
            container.Clear();
            container.Add(dropdown);
            if (property.managedReferenceValue != null)
            {
                var field = new PropertyField(property);
                field.BindProperty(property);
                container.Add(field);
            }
        });

        container.Add(dropdown);

        if (property.managedReferenceValue != null)
        {
            var field = new PropertyField(property);
            field.BindProperty(property);
            container.Add(field);

            // Walk through all child properties of the field
            //var iterator = property.Copy();
            //var end = iterator.GetEndProperty();

            //if (iterator.NextVisible(true))
            //{
            //    do
            //    {
            //        if (SerializedProperty.EqualContents(iterator, end))
            //        {
            //            break;
            //        }

            //        //if (iterator.name == "m_Script") // skip Unity’s script ref
            //        //    continue;

            //        var field = new PropertyField(iterator);
            //        field.BindProperty(iterator);
            //        //field.style.marginLeft = 10;
            //        container.Add(field);

            //    } while (iterator.NextVisible(false));
            //}
        }

        return container;
    }

    static void BuildTypeMap()
    {
        typeMap = TypeCache.GetTypesDerivedFrom<PowerUpConfig>()
            .ToDictionary(t => t.Name, t => t);
        //ObjectNames.NicifyVariableName(t.Name)
    }

    static string GetShortTypeName(string fullTypeName)
    {
        if (string.IsNullOrEmpty(fullTypeName)) return null;
        var parts = fullTypeName.Split(' ');
        return parts.Length > 1 ? parts[1].Split('.').Last() : fullTypeName;
    }
}