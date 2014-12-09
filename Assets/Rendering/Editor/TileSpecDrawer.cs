using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (TileSpec))]
public class TileSpecDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight(prop);
	}

	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative("name").stringValue = "Tile Name";       
		prop.serializedObject.ApplyModifiedProperties();
	}

	
	public static float calculateHeight(SerializedProperty prop){
		float h = EditorUtil.row;
		if (!prop.FindPropertyRelative ("folded").boolValue) {
			SerializedProperty renders = prop.FindPropertyRelative("renders");
			for (int i = 0; i < renders.arraySize; i++)
				h += TileRenderDrawer.calculateHeight(renders.GetArrayElementAtIndex(i));
			h += 4 * EditorUtil.row;
		}
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		
		SerializedProperty name = prop.FindPropertyRelative ("name");

		SerializedProperty folded = prop.FindPropertyRelative ("folded");
		bool fold = folded.boolValue;
		

		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorUtil.textField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), name, !fold, "Tile Name");

		EditorGUI.indentLevel++;
		SerializedProperty context = prop.FindPropertyRelative ("context");
		if (!fold){
			SerializedProperty renders = prop.FindPropertyRelative("renders");
			float w = pos.width;
			float h = EditorUtil.row;

			if (EditorUtil.plus(pos.x + 12 * EditorGUI.indentLevel + EditorUtil.buttonSize, pos.y + EditorUtil.row, "Add Tile Render Data")){
			//	TileRender.construct(renders.GetArrayElementAtIndex(renders.arraySize++));
			}
			if (EditorUtil.minus(pos.x + 12 * EditorGUI.indentLevel , pos.y + EditorUtil.row, "Remove Tile Render Data")){
				if (renders.arraySize > 1)
					renders.GetArrayElementAtIndex(renders.arraySize--);
			}
			if (renders.arraySize == 0){
				renders.arraySize++;
			}
			for (int i = 0; i < renders.arraySize; i++){
				SerializedProperty render = renders.GetArrayElementAtIndex(i);
				EditorGUI.PropertyField (new Rect (pos.x, pos.y + h + EditorUtil.row, w, h), render, GUIContent.none);
				h += TileRenderDrawer.calculateHeight(render);
			}
			//


			//

			EditorGUIUtility.labelWidth = 100;
		
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


			EditorGUIUtility.labelWidth = 0;
		}
		EditorGUI.indentLevel--;

		prop.serializedObject.ApplyModifiedProperties ();
	}


}
