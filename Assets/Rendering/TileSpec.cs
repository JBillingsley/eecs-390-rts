using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TileSpec {

	public string name = "Tile Name";
//	public TileContext context;
//	public int index;

	[SerializeField]
	private List<TileRender> renders = new List<TileRender>();

	public bool solid;
	public bool diggable;
	public Element resource;
	[Range (0, 8)]
	public int resourceQuantity;
	[Range (1, 255)]
	public byte durability;
	[Range (0, 32)]
	public int weight;

	[SerializeField]
	private bool folded;


	public TileRender getRender(int index){
		if (renders.Count < 1){
			renders.Add(new TileRender());
		}
		int i = index % renders.Count;
		return renders [i];
	}


}
