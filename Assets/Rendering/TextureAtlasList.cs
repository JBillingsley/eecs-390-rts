using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TextureAtlasList {

	public static TextureAtlasList list = new TextureAtlasList();

	public List<TextureAtlas> textureAtlases = new List<TextureAtlas>();
	[SerializeField]
	private bool textureAtlasFolded;

	private TextureAtlasList(){
		textureAtlases.Add (new TextureAtlas());
	}

	public static string[] getTextureAtlasNames(){
		string[] names = new string[list.textureAtlases.Count];
		int i = 0;
		foreach (TextureAtlas t in list.textureAtlases)
			names[i++] = t.name;
		return names;
	}
	
	public static TextureAtlas getTextureAtlas(int i){
		if (i >= 0 && i < list.textureAtlases.Count)
			return list.textureAtlases[i];
		return null;
	}
	
	public static TextureAtlas getTextureAtlas(string name){
		foreach (TextureAtlas t in list.textureAtlases)
			if (t.name.Equals (name))
				return t;
		return null;
	}
}
