using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {

	int listSize = 0;
	bool showList = false;

	public override void OnInspectorGUI(){
		Map map = (Map)target;

		GUI.enabled = !Application.isPlaying;

		EditorGUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = 40;
		map.w = EditorGUILayout.IntField("Width", Mathf.Clamp(map.w, 1, 32));
		EditorGUIUtility.labelWidth = 45;
		map.h = EditorGUILayout.IntField("Height", Mathf.Clamp(map.h, 1, 32));
		EditorGUILayout.EndHorizontal();

		EditorGUIUtility.labelWidth = 80;
		map.tileSize = EditorGUILayout.IntField("Tile Size", Mathf.NextPowerOfTwo(Mathf.Clamp(map.tileSize, 1, 64)));
		map.chunkSize = EditorGUILayout.IntField("Chunk Size", Mathf.NextPowerOfTwo(Mathf.Clamp(map.chunkSize, 1, 32)));

		map.surfaceHeight = EditorGUILayout.IntField("Height", Mathf.Clamp(map.surfaceHeight,1,map.h * map.chunkSize));
		map.surfaceRandomness = EditorGUILayout.IntField("Randomness", Mathf.Clamp(map.surfaceRandomness,1,5));

		EditorGUI.indentLevel++;
		EditorGUIUtility.labelWidth = 150;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("rects"),true);
		serializedObject.ApplyModifiedProperties();
		EditorGUI.indentLevel--;

		EditorGUIUtility.labelWidth = 80;

		GUI.enabled = true;

		map.setRenderMode((byte)EditorGUILayout.Popup("RenderMode", map.renderMode, new string[]{"Terrain", "Navmesh", "Background"}));

		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("tileset"), true);
	
		serializedObject.ApplyModifiedProperties();
		EditorGUIUtility.LookLikeControls();
		EditorGUI.indentLevel--;

	}

	public void displayRectList(Map map){
		for(int i = 0; i < map.rects.Count; i++){
			TileCluster currentRect = map.rects[i];

			EditorGUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = 20;
			EditorGUILayout.IntField("X",currentRect.x);
			EditorGUIUtility.labelWidth = 20;
			EditorGUILayout.IntField("Y",currentRect.y);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = 40;
			EditorGUILayout.IntField("Width",currentRect.width);
			EditorGUIUtility.labelWidth = 45;
			EditorGUILayout.IntField("Height",currentRect.height);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
	}
	
	
}