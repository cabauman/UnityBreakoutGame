using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.UIElements;

namespace BreakoutGame
{
    [CustomPropertyDrawer(typeof(PowerUpConfig), true)]
    public class PowerUpDrawerBackup : PropertyDrawer
    {
        static Dictionary<string, Type> _typeMap;
        static List<string> _options;
        static readonly string DefaultDropdownOption = "Select PowerUp Type";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (_typeMap == null) BuildTypeMap();

            var container = new VisualElement();

            var typeName = property.managedReferenceFullTypename;
            var displayName = GetShortTypeName(typeName) ?? DefaultDropdownOption;

            var dropdown = new DropdownField("PowerUp Type", _options, displayName);
            dropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue == DefaultDropdownOption)
                {
                    property.managedReferenceValue = null;
                }
                else
                {
                    var selectedType = _typeMap[evt.newValue];
                    property.managedReferenceValue = Activator.CreateInstance(selectedType);
                }

                property.serializedObject.ApplyModifiedProperties();

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
                //var field = new PropertyField(property);
                //field.BindProperty(property);
                //container.Add(field);

                var iterator = property.Copy();
                var end = iterator.GetEndProperty();

                if (iterator.NextVisible(true))
                {
                    do
                    {
                        if (SerializedProperty.EqualContents(iterator, end))
                        {
                            break;
                        }

                        var field = new PropertyField(iterator);
                        field.BindProperty(iterator);
                        //field.style.marginLeft = 10;
                        container.Add(field);

                    } while (iterator.NextVisible(false));
                }
            }

            return container;
        }

        static void BuildTypeMap()
        {
            _typeMap = TypeCache.GetTypesDerivedFrom<PowerUpConfig>()
                .ToDictionary(t => t.Name, t => t);
            _options = _typeMap.Keys
                .Prepend(DefaultDropdownOption)
                .ToList();
            //ObjectNames.NicifyVariableName(t.Name)
        }

        static string GetShortTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName)) return null;
            var parts = fullTypeName.Split(' ');
            return parts.Length > 1 ? parts[1].Split('.').Last() : fullTypeName;
        }
    }
}