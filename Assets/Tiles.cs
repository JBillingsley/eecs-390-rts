using UnityEngine;
using System.Collections.Generic;

public class Tiles : MonoBehaviour {

	public TextureAtlas tileset;
	public TileSpecList list = TileSpecList.list;

	private static List<TileSpec> makeList(){
		List<TileSpec> list = new List<TileSpec>();
		list.Add(new TileSpec());
		return list;
	}

}
