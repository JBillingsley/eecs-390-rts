using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {
	
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

		GUI.enabled = true;

		map.setRenderMode((byte)EditorGUILayout.Popup("RenderMode", map.renderMode, new string[]{"Terrain", "Navmesh", "Background"}));

		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("tileset"), true);
	
		serializedObject.ApplyModifiedProperties();
		EditorGUIUtility.LookLikeControls();
		EditorGUI.indentLevel--;

	}
	
	
	
}