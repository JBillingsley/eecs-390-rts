using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (TileRender))]
public class TileRenderDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight(prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		float h = EditorUtil.row;
		if (!prop.FindPropertyRelative ("folded").boolValue) {
			SerializedProperty context = prop.FindPropertyRelative ("context");
			float p = TileSpec.previewSize((TileContext)context.enumValueIndex);
			h += Mathf.Max (p + EditorUtil.padding, 2 * EditorUtil.row);
		}
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		
		SerializedProperty folded = prop.FindPropertyRelative ("folded");
		bool fold = folded.boolValue;
		
		SerializedProperty context = prop.FindPropertyRelative ("context");
		SerializedProperty index = prop.FindPropertyRelative ("index");

		float p = TileRender.previewSize((TileContext)context.enumValueIndex);
		float h = Mathf.Max (p + EditorUtil.padding, 2 * EditorUtil.row);
		float w = pos.width;

		//
		EditorGUIUtility.labelWidth = 100;

		EditorGUI.BeginChangeCheck();

		EditorGUI.PropertyField (new Rect (pos.x, pos.y + h/2, w - p - EditorUtil.padding, EditorUtil.height), context, new GUIContent("Context"));
		EditorGUI.PropertyField (new Rect (pos.x, pos.y + h/2 + EditorUtil.row, w - p - EditorUtil.padding, EditorUtil.height), index, new GUIContent("Index"));

		if (EditorGUI.EndChangeCheck ()) {
			prop.FindPropertyRelative("view").objectReferenceValue = TileSpec.constructPreview(prop);
		}

		Rect r = new Rect (pos.x + pos.width - p, pos.y + EditorUtil.row + (h - p - EditorUtil.padding)/2, p, p);
		Texture2D tex = (Texture2D)prop.FindPropertyRelative("view").objectReferenceValue;
		GUI.Box(r, tex == null? new GUIContent("Error"):new GUIContent(tex));
		
		EditorGUIUtility.labelWidth = 0;

		prop.serializedObject.ApplyModifiedProperties ();
	}


}
