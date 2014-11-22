using UnityEngine;
using System.Collections;

[System.Serializable]
public struct TileCluster
{
	public int x;
	public int y;
	public int width;
	public int height;

	public int tileType;
	public float likeliness;

	public bool background;
}

