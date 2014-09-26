using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Global : MonoBehaviour{
	public static Global instance;

	public List<Tileset> tilesets = new List<Tileset>();


	protected Global(){
		if (instance == null)
			instance = this;
		tilesets.Add (new Tileset());
	}

	public static Tileset getTileset(string name){
		foreach (Tileset t in instance.tilesets)
			if (t.name.Equals (name))
				return t;
		return null;
	}

}
