using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (TileSpec))]
public class TileSpecDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight(prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		float h = EditorUtil.row;
		if (!prop.FindPropertyRelative ("folded").boolValue)
			h += Mathf.Max(3 * EditorUtil.row, TileSpec.previewSize((TileContext)prop.FindPropertyRelative("context").enumValueIndex) + EditorUtil.padding);
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty context = prop.FindPropertyRelative ("context");
		SerializedProperty index = prop.FindPropertyRelative ("index");
		SerializedProperty solid = prop.FindPropertyRelative ("solid");

		SerializedProperty folded = prop.FindPropertyRelative ("folded");
		bool fold = folded.boolValue;



		if (name == null || context == null || index == null)
			return;

		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorUtil.textField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), name, !fold, "Tile Name");

		EditorGUI.indentLevel++;
		if (!fold){
			float preview = TileSpec.previewSize((TileContext)context.enumValueIndex);
			float h = Mathf.Max (preview, 3 * EditorUtil.row);
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h / 2 - EditorUtil.row/2, pos.width - preview - EditorUtil.padding, EditorUtil.height), context, new GUIContent("Context"));
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h / 2 + EditorUtil.row/2, pos.width - preview - EditorUtil.padding, EditorUtil.height), index, new GUIContent("Index"));
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h / 2 + 3 * EditorUtil.row/2, pos.width - preview - EditorUtil.padding, EditorUtil.height), solid, new GUIContent("Solid"));
			Rect r = new Rect (pos.x + pos.width - preview, pos.y + EditorUtil.row, preview, preview);
			GUI.Box(r, GUIContent.none);
		}
		EditorGUI.indentLevel--;

		prop.serializedObject.ApplyModifiedProperties ();
	}


}
