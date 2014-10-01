using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GlobalData {
	private static GlobalData instance = new GlobalData();
	public static GlobalData inst{
		get {
			if (instance == null)
				instance = new GlobalData();
			return instance;
		}
	}

	public List<Tileset> tilesets = new List<Tileset>();
	public List<AnimationSet> animationsets = new List<AnimationSet>();
	[SerializeField]
	private bool tilesetFolded;
	[SerializeField]
	private bool animationsetFolded;

	protected GlobalData(){
		if (instance == null)
			instance = this;
		tilesets.Add (new Tileset());
		animationsets.Add (new AnimationSet());
	}

	public static string[] getTilesetNames(){
		string[] names = new string[inst.tilesets.Count];
		int i = 0;
		foreach (Tileset t in instance.tilesets)
			names[i++] = t.name;
		return names;
	}

	public static Tileset getTileset(int i){
		return instance.tilesets[i];
	}

	public static Tileset getTileset(string name){
		foreach (Tileset t in instance.tilesets)
			if (t.name.Equals (name))
				return t;
		return null;
	}

}
