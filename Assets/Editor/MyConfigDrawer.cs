using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(MyConfigAttribute))]
public class MyConfigDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        // --- Header ---
        //var header = new Label("Config");
        //header.style.unityFontStyleAndWeight = FontStyle.Bold;
        //header.style.marginBottom = 4;
        //container.Add(header);

        // Walk through all child properties of the field
        var iterator = property.Copy();
        var end = iterator.GetEndProperty();

        if (iterator.NextVisible(true))
        {
            do
            {
                if (SerializedProperty.EqualContents(iterator, end))
                    break;

                //if (iterator.name == "m_Script") // skip Unity’s script ref
                //    continue;

                var field = new PropertyField(iterator);
                field.BindProperty(iterator);
                //field.style.marginLeft = 10;
                container.Add(field);

            } while (iterator.NextVisible(false));
        }

        return container;
    }
}
