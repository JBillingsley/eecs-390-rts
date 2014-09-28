using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Global : MonoBehaviour{
	public static Global instance;

	public List<Tileset> tilesets = new List<Tileset>();
	public List<AnimationSet> animationSets = new List<AnimationSet>();


	protected void Awake(){
		if (instance == null)
			instance = this;
		tilesets.Add (new Tileset());
		animationSets.Add (new AnimationSet());
	}

	public static Tileset getTileset(string name){
		foreach (Tileset t in instance.tilesets)
			if (t.name.Equals (name))
				return t;
		return null;
	}

}
