using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

/// <summary>
/// The property drawer for the <see cref="TypeDropdownAttribute"/>
/// </summary>
[CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
public class TypeDropdownDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		TypeDropdownAttribute typeAttribute = attribute as TypeDropdownAttribute;
		Type baseType = typeAttribute.BaseType;

		string[] allTypes = Reflect.AllTypeStringsFrom(baseType).ToArray();

		if(allTypes.Length <= 0)
		{
			EditorGUI.Popup(position, label.text, 0, new string[] { "No types of " + baseType.Name + " found" });
		}
		else
		{
			Array.Sort(allTypes);

			SerializedProperty stringProperty = null;

			if(property.propertyType == SerializedPropertyType.String)
			{
				stringProperty = property;
			}

			if(stringProperty != null)
			{
				string currentType = stringProperty.stringValue;

				int selected = string.IsNullOrEmpty(stringProperty.stringValue) ? 0 : Array.IndexOf(allTypes, currentType);
				int newSelection = EditorGUI.Popup(position, label.text, selected, allTypes);

				stringProperty.stringValue = allTypes[Mathf.Max(0, newSelection)];
			}
			else
			{
				GUI.Label(position, "The TypeDropDownAttribute only works on strings.");
			}
		}

		EditorGUI.EndProperty();
	}
	}
