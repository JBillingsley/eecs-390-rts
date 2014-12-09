using UnityEngine;
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



}
