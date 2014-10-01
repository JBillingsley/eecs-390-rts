using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class AnimationSet {

	public string name = "Animation Set Name";
	public string tilesetName = "Tileset Name";
	public Animation[] animations = new Animation[1];
	[SerializeField]
	private int tileset = 0;
	[SerializeField]
	private float lastFrame = Time.realtimeSinceStartup;

	[SerializeField]
	private bool folded;
	[SerializeField]
	private int preview;
	[SerializeField]
	private int previewIndex;

	public Tileset getTileset(){
		return GlobalData.getTileset (tileset);
	}

	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative("name").stringValue = "Animation Set Name";                
		prop.FindPropertyRelative ("tilesetName").stringValue = "Tileset Name";      
		SerializedProperty animations = prop.FindPropertyRelative ("animations");
		animations.arraySize = 1;
		Animation.construct (animations.GetArrayElementAtIndex(0));
		prop.serializedObject.ApplyModifiedProperties();
	}

}
