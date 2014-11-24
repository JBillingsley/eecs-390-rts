using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (TileSpec))]
public class TileSpecDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight(prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		float h = EditorUtil.row;
		if (!prop.FindPropertyRelative ("folded").boolValue) {
			SerializedProperty context = prop.FindPropertyRelative ("context");
			float p = TileSpec.previewSize((TileContext)context.enumValueIndex);
			h += Mathf.Max (p + EditorUtil.padding, 2 * EditorUtil.row) + 3 * EditorUtil.row;
		}
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		
		SerializedProperty name = prop.FindPropertyRelative ("name");

		SerializedProperty folded = prop.FindPropertyRelative ("folded");
		bool fold = folded.boolValue;
		
		if (name == null)
			return;

		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorUtil.textField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), name, !fold, "Tile Name");

		EditorGUI.indentLevel++;
		SerializedProperty context = prop.FindPropertyRelative ("context");
		if (!fold){
			float p = TileSpec.previewSize((TileContext)context.enumValueIndex);
			float h = Mathf.Max (p + EditorUtil.padding, 2 * EditorUtil.row);
			float w = pos.width;

			//
			EditorGUIUtility.labelWidth = 100;

			EditorGUI.BeginChangeCheck();

			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h/2, w - p - EditorUtil.padding, EditorUtil.height), context, new GUIContent("Context"));
			SerializedProperty index = prop.FindPropertyRelative ("index");
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h/2 + EditorUtil.row, w - p - EditorUtil.padding, EditorUtil.height), index, new GUIContent("Index"));

			if (EditorGUI.EndChangeCheck ()) {
				prop.FindPropertyRelative("view").objectReferenceValue = TileSpec.constructPreview(prop);
			}

			SerializedProperty weight = prop.FindPropertyRelative ("weight");
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h + EditorUtil.row, w/2, EditorUtil.height), weight, new GUIContent("Weight"));
			SerializedProperty durability = prop.FindPropertyRelative ("durability");
			EditorGUI.PropertyField (new Rect (pos.x + w/2, pos.y + h + EditorUtil.row, w/2, EditorUtil.height), durability, new GUIContent("Durability"));

			//
			SerializedProperty solid = prop.FindPropertyRelative ("solid");
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h + EditorUtil.row * 2, 130, EditorUtil.height), solid, new GUIContent("Solid"));
			SerializedProperty diggable = prop.FindPropertyRelative ("diggable");
			EditorGUI.PropertyField (new Rect (pos.x + w - 130, pos.y + h + EditorUtil.row * 2, 130, EditorUtil.height), diggable, new GUIContent("Diggable"));

			SerializedProperty resource = prop.FindPropertyRelative ("resource");
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + h + EditorUtil.row * 3, w - 110, EditorUtil.height), resource, new GUIContent("Resource"));
			SerializedProperty resourceQuantity = prop.FindPropertyRelative ("resourceQuantity");
			EditorGUI.PropertyField (new Rect (pos.x + w - 130, pos.y + h + EditorUtil.row * 3, 130, EditorUtil.height), resourceQuantity, new GUIContent("Quantity"));


			Rect r = new Rect (pos.x + pos.width - p, pos.y + EditorUtil.row + (h - p - EditorUtil.padding)/2, p, p);
			Texture2D tex = (Texture2D)prop.FindPropertyRelative("view").objectReferenceValue;
			GUI.Box(r, tex == null? new GUIContent("Error"):new GUIContent(tex));
			
			EditorGUIUtility.labelWidth = 0;
		}
		EditorGUI.indentLevel--;

		prop.serializedObject.ApplyModifiedProperties ();
	}


}
