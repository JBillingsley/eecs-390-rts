using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tileset))]
public class TilesetEditor : Editor {
	
	public override void OnInspectorGUI(){
		Tileset t = (Tileset)target;
		
		GUI.enabled = !Application.isPlaying;
		
		EditorGUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = 40;
		t.setWidth(EditorGUILayout.IntField("Width", Mathf.Max(t.width, 1)));
		EditorGUIUtility.labelWidth = 45;
		t.setHeight(EditorGUILayout.IntField("Height", Mathf.Max(t.height, 1)));
		EditorGUILayout.EndHorizontal();

		t.setTexture((Texture)EditorGUI.ObjectField(new Rect(3,3,200,20), "Tileset Texture", t.texture, typeof(Texture), true));
	}
	
	
	
}