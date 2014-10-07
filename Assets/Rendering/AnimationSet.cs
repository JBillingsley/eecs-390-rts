using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class AnimationSet {

	public string name = "Animation Set Name";
	public Animation[] animations = new Animation[1];
	[SerializeField]
	private int textureAtlasID = 0;
	[SerializeField]
	private float lastFrame;

	[SerializeField]
	private bool folded;
	[SerializeField]
	private int preview;
	[SerializeField]
	private int previewIndex;

	public TextureAtlas getTileset(){
		return TextureAtlasList.getTextureAtlas(textureAtlasID);
	}

	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative("name").stringValue = "Animation Set Name";         
		SerializedProperty animations = prop.FindPropertyRelative ("animations");
		animations.arraySize = 1;
		Animation.construct (animations.GetArrayElementAtIndex(0));
		prop.serializedObject.ApplyModifiedProperties();
	}

}
