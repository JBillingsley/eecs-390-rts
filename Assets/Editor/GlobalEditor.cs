using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Global))]
public class GlobalEditor : Editor {
	
	public override void OnInspectorGUI(){
		Global g = (Global)target;
		serializedObject.Update();

		GUI.enabled = !Application.isPlaying;

		renderTilesets ();
		renderAnimationsets ();

		serializedObject.ApplyModifiedProperties();
	}

	private void renderTilesets(){
		SerializedProperty list = serializedObject.FindProperty ("tilesets");
		EditorGUILayout.PropertyField(list);

		EditorGUI.indentLevel++;
		if (list.isExpanded){
			for (int index = 0; index < list.arraySize; index++){
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(index), true);
				Rect lr = GUILayoutUtility.GetLastRect();
				float dx = EditorGUI.indentLevel * 8 + TilesetDrawer.padding;
				Rect r = new Rect (lr.x + dx, lr.y + 4 * TilesetDrawer.dy, lr.width - TilesetDrawer.texSize - lr.x - TilesetDrawer.padding - dx, TilesetDrawer.dy);
				if (GUI.Button (r, "Delete Tileset")) {
					list.DeleteArrayElementAtIndex(index);
				}
			}
			if (GUILayout.Button ("Add Tileset")) {
				int i = list.arraySize++;
				SerializedProperty tileSet = list.GetArrayElementAtIndex(i);
				tileSet.FindPropertyRelative("width").intValue = 1;
				tileSet.FindPropertyRelative("height").intValue = 1;
				tileSet.FindPropertyRelative("name").stringValue = "Tileset Name";
				tileSet.serializedObject.ApplyModifiedProperties();
			}
			
			EditorGUIUtility.LookLikeControls();
		}
		EditorGUI.indentLevel--;
	}
	
	private void renderAnimationsets(){
		SerializedProperty list = serializedObject.FindProperty ("animationSets");
		EditorGUILayout.PropertyField(list);
		
		EditorGUI.indentLevel++;
		if (list.isExpanded){
			for (int index = 0; index < list.arraySize; index++){
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(index), true);/*
				Rect lr = GUILayoutUtility.GetLastRect();
				Rect r = new Rect (lr.x, lr.y + 4 * TilesetDrawer.dy, lr.width - TilesetDrawer.texSize - lr.x - TilesetDrawer.padding, TilesetDrawer.dy);
				if (GUI.Button (r, "Delete Tileset")) {
					list.DeleteArrayElementAtIndex(index);
				}*/
			}
			if (GUILayout.Button ("Add Animation Set")) {
				int i = list.arraySize++;/*
				SerializedProperty tileSet = list.GetArrayElementAtIndex(i);
				tileSet.FindPropertyRelative("width").intValue = 1;
				tileSet.FindPropertyRelative("height").intValue = 1;
				tileSet.FindPropertyRelative("name").stringValue = "Tileset Name";
				tileSet.serializedObject.ApplyModifiedProperties();*/
			}
			
			EditorGUIUtility.LookLikeControls();
		}
		EditorGUI.indentLevel--;
	}
}