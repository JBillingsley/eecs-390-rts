using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(FixedAttribute))]
public class FixedDrawer : PropertyDrawer {
	private static GUIStyle style;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		if (style == null){
			style = new GUIStyle((GUIStyle)"TextField");
		}
		FixedAttribute range = attribute as FixedAttribute;
		EditorGUI.BeginChangeCheck ();
		property.intValue = EditorGUI.IntField(position, property.intValue, style);
		if (EditorGUI.EndChangeCheck())
			property.serializedObject.ApplyModifiedProperties();
	}
}
