using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (TextureAtlas))]
public class TextureAtlasDrawer : PropertyDrawer {

	public const int padding = 10;

	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight(prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		if (prop.FindPropertyRelative ("folded").boolValue)
			return EditorUtil.row;
		return EditorUtil.texSize + EditorUtil.padding;
	}

	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {

		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty width = prop.FindPropertyRelative ("width");
		SerializedProperty height = prop.FindPropertyRelative ("height");
		SerializedProperty texture = prop.FindPropertyRelative ("texture");
		
		SerializedProperty folded = prop.FindPropertyRelative ("folded");
		bool fold = folded.boolValue;
		if (name == null || width == null || height == null)
			return;

		EditorGUIUtility.labelWidth = 80;

		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
				SerializedProperty context = prop.FindPropertyRelative ("context");

		EditorGUI.indentLevel++;
		EditorUtil.textField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - EditorUtil.texSize - padding, EditorUtil.height), name, !fold);
		if (!fold){
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + 1*EditorUtil.row, pos.width - EditorUtil.texSize - padding, EditorUtil.height), texture, new GUIContent("Texture"));
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + 2*EditorUtil.row, pos.width - EditorUtil.texSize - padding, EditorUtil.height), width, new GUIContent("Width"));
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + 3*EditorUtil.row, pos.width - EditorUtil.texSize - padding, EditorUtil.height), height, new GUIContent("Height"));
			GUI.Box (new Rect ( pos.x + pos.width - EditorUtil.texSize, pos.y, EditorUtil.texSize, EditorUtil.texSize), (Texture2D)texture.objectReferenceValue);
		}
		EditorGUI.indentLevel--;

		EditorGUIUtility.labelWidth = 0;

		prop.serializedObject.ApplyModifiedProperties ();
	}
	
}
