using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class AnimationSet {

	public string name = "Animation Set Name";
	public string tilesetName = "Tileset Name";
	public Animation[] animations = new Animation[1];

	private Tileset tileset;

	[SerializeField]
	private bool folded;
	[SerializeField]
	private int preview;
	[SerializeField]
	private int previewIndex;

	public Tileset getTileset(){
		if (tileset == null)
			tileset = Global.getTileset (tilesetName);
		return tileset;
	}

	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative("name").stringValue = "Animation Set Name";                
		prop.FindPropertyRelative ("tilesetName").stringValue = "Tileset Name";      
		prop.FindPropertyRelative ("animations").arraySize = 1;
		Animation.construct (prop.FindPropertyRelative ("animations").GetArrayElementAtIndex(0));
		prop.serializedObject.ApplyModifiedProperties();
	}

}
