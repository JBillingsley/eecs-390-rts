using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParentedNode {

	public static Map m;
	public ParentedNode parent;
	//public NavigationNode node;
	public Vector2 location;
	public float weight;

	public ParentedNode(ParentedNode parent,Vector2 node, float weight){
		this.parent = parent;
		this.weight = weight;
		if(parent != null){
			this.weight += parent.weight;
		}
		this.location = node;
	}

	public override string ToString ()
	{
		string s = location + " " + weight;
		return s; //s
	}

	public Vector2[] GetNeighbors(){

		List<Vector2> neighbors = new List<Vector2>();
		IVector2 pos = new IVector2(location.x,location.y);

		if(m == null){
			m = GameObject.FindObjectOfType<Map>();
		}


		int[] dirs = new int[]{Direction.TOP, Direction.BOTTOM, Direction.LEFT, Direction.RIGHT, Direction.TOPLEFT, Direction.TOPRIGHT, Direction.BOTTOMLEFT, Direction.BOTTOMRIGHT};

		bool[] passability = Direction.extractByte(dirs, (byte)(~m.getByte(pos, Map.NAVIGATION_MAP)));
		bool[] solid = Direction.extractByte(dirs, (byte)(~m.getByte(pos, Map.FOREGROUND_CONTEXT)));
		IVector2[] directions = Direction.getDirections(dirs);

		for (int i = 0; i < 4; i++){
			if(passability[i])
				neighbors.Add (pos + directions[i]);
		}
		for (int i = 4; i < 6; i++){
			if(passability[i] && solid[0])
				neighbors.Add (pos + directions[i]);
		}
		for (int i = 6; i < 8; i++){
			if(passability[i] && solid[i - 4])
				neighbors.Add (pos + directions[i]);
		}
		return neighbors.ToArray();
	}

	public Vector2[] GetDigNeighbors(){
		
		List<Vector2> neighbors = new List<Vector2>();
		List<Vector2> goodNeighbors = new List<Vector2>();
		IVector2 pos = new IVector2(location.x,location.y);

		neighbors.Add (new IVector2(location.x - 1, location.y));
		neighbors.Add (new IVector2(location.x + 1, location.y));
		neighbors.Add (new IVector2(location.x, location.y -1));
		neighbors.Add (new IVector2(location.x, location.y + 1));

		foreach(IVector2 v in neighbors){
			byte b = m.getByte(v,Map.FOREGROUND_ID);
			TileSpec t = TileSpecList.getTileSpec(b);
			if(t.diggable){
				goodNeighbors.Add(v);
			}
		}

		return goodNeighbors.ToArray();
	}
}
