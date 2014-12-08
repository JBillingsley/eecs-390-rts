using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileSpecList{
	
	public static TileSpecList list = new TileSpecList();
	
	public TextureAtlas tileset;
	public List<TileSpec> tilespecs = new List<TileSpec>();

	[SerializeField]
	private bool tileSpecFolded;

	private TileSpecList(){
		tilespecs.Add (new TileSpec());
	}
	
	public static string[] getTileSpecNames(){
		string[] names = new string[list.tilespecs.Count];
		int i = 0;
		foreach (TileSpec t in list.tilespecs)
			names[i++] = t.name;
		return names;
	}
	
	public static TileSpec getTileSpec(int i){
		if (i >= 0 && i < list.tilespecs.Count)
			return list.tilespecs[i];
		return null;
	}
	
	public static TileSpec getTileSpec(string name){
		foreach (TileSpec t in list.tilespecs)
			if (t.name.Equals (name))
				return t;
		return null;
	}
	public static int getTileSpecInt (string name){
		for(int i = 0; i < list.tilespecs.Count; i++){
			if(list.tilespecs[i].name.Equals(name)){
				return i;
			}
		}
		return 0;
	}
}
