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
		//string s = node.transform.position + " " + weight;
		return ""; //s
	}

	public Vector2[] GetNeighbors(){

		List<Vector2> neighbors = new List<Vector2>();
		IVector2 pos = new Vector2((int)location.x,(int)location.y);

		if(m == null){
			m = GameObject.FindObjectOfType<Map>();
		}

		//Clockwise from top, top,right,down,left
		int[] dirs = new int[]{Direction.TOP, Direction.RIGHT, Direction.BOTTOM, Direction.LEFT};
		IVector2[] directions = Direction.getDirections(dirs);

		byte adj = m.getTileAdjacency((int)location.x,(int)location.y);
		bool[] passability = Direction.extractByte(dirs, adj);

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
