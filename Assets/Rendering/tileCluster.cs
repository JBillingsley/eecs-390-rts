using UnityEngine;
using System.Collections;

[System.Serializable]
public struct TileCluster
{
	public string description;

	public int x;
	public int y;
	public int width;
	public int height;

	public int tileType;
	public float likeliness;

	public bool background;

	public TileCluster copy(){
		TileCluster tc = this;
		TileCluster t = new TileCluster();
		t.background = tc.background;
		t.description = tc.description;
		t.height = tc.height;
		t.likeliness = tc.likeliness;
		t.tileType = tc.tileType;
		t.width = tc.width;
		t.x = tc.x;
		t.y = tc.y;
		return t;
	}
}

