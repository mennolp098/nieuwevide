using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomPropertyDrawer(typeof(ScriptableObjectDropdownAttribute))]
public class ScriptableObjectDropdownDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		ScriptableObjectDropdownAttribute typeAttribute = attribute as ScriptableObjectDropdownAttribute;
		String baseObject = typeAttribute.scriptableObject;

		string[] guids = AssetDatabase.FindAssets ("t:" + baseObject);
		string[] assetNames = new string[guids.Length];
		string[] names = new string[guids.Length];
		for (int i = 0; i < guids.Length; i++) {
			string assetPath = AssetDatabase.GUIDToAssetPath (guids[i]);
			string[] splittedPath = assetPath.Split ("/" [0]);
			assetNames [i] = splittedPath [splittedPath.Length - 1];
			names [i] = assetNames [i].Split ("." [0]) [0];
		}
			 
		if(names.Length <= 0)
		{
			EditorGUI.Popup(position, label.text, 0, new string[] { "No types of " + baseObject + " found" });
		}
		else
		{
			SerializedProperty stringProperty = null;

			if(property.propertyType == SerializedPropertyType.String)
			{
				stringProperty = property;
			}
			if (stringProperty != null) {
				string currentName = stringProperty.stringValue;
				int selected = string.IsNullOrEmpty(stringProperty.stringValue) ? 0 : Array.IndexOf(names, currentName);
				int newSelection = EditorGUI.Popup (position, label.text, selected, names);

				stringProperty.stringValue = names[Mathf.Max(0, newSelection)];
			}
		}

		EditorGUI.EndProperty();
	}
}

