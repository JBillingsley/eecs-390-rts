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

		//Clockwise from top, top,right,down,left
		int[] dirs = new int[]{Direction.TOP, Direction.RIGHT, Direction.BOTTOM, Direction.LEFT};
		IVector2[] directions = Direction.getDirections(dirs);

		byte adj = m.getTileNav(pos);
		bool[] passability = Direction.extractByte(dirs, (byte)(~adj));

		if(passability[0]){
			neighbors.Add (pos + directions[0]);
		}
		if(passability[1]){
			neighbors.Add (pos + directions[1]);
		}
		if(passability[2]){
			neighbors.Add (pos + directions[2]);
		}
		if(passability[3]){
			neighbors.Add (pos + directions[3]);
		}

		return neighbors.ToArray();
	}
}
