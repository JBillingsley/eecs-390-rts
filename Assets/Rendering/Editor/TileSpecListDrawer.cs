using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer (typeof (TileSpecList))]
public class TileSpecListDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight (prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		float h = EditorUtil.row;
		if (!prop.FindPropertyRelative ("tileSpecFolded").boolValue){
			SerializedProperty tilespecs = prop.FindPropertyRelative ("tilespecs");
			for (int i = 0; i < tilespecs.arraySize; i++) {
				SerializedProperty tilespec = tilespecs.GetArrayElementAtIndex(i);
				h += TileSpecDrawer.calculateHeight(tilespec);
			}
		}
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		
		renderTileSpecs(pos, prop);
		
		
	}
	
	private float renderTileSpecs(Rect pos, SerializedProperty prop){
		SerializedProperty folded = prop.FindPropertyRelative ("tileSpecFolded");
		bool fold = folded.boolValue;
		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorGUI.LabelField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), new GUIContent("Tiles"));
		
		SerializedProperty tilespecs = prop.FindPropertyRelative ("tilespecs");
		if (EditorUtil.plus(pos.x + pos.width - EditorUtil.buttonSize, pos.y, "New Tile Specification"))
			TileSpec.construct(tilespecs.GetArrayElementAtIndex(tilespecs.arraySize++));
		
		float ay = EditorUtil.row;
		if (!fold){
			EditorGUI.indentLevel++;
			float[] h = new float[tilespecs.arraySize];
			for (int i = 0; i < tilespecs.arraySize; i++) {
				SerializedProperty tilespec = tilespecs.GetArrayElementAtIndex(i);
				float tilespecHeight = TileSpecDrawer.calculateHeight(tilespec);
				
				//Draw Animation Set
				EditorGUI.PropertyField (new Rect (pos.x, pos.y + ay, pos.width, tilespecHeight), tilespec, GUIContent.none);
				if (EditorUtil.ex(pos.x + pos.width - EditorUtil.buttonSize, pos.y + ay, "Delete Tile Specification"))
					tilespecs.DeleteArrayElementAtIndex(i);

				h[i] = ay;
				ay += tilespecHeight;
			}
			EditorUtil.foldLines(pos.x + EditorGUI.indentLevel * 12, pos.y, h);
			EditorGUI.indentLevel--;
		}
		return ay;
	}
}