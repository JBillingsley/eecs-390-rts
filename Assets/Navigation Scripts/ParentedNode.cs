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
		Vector2 pos = new Vector2((int)location.x,(int)location.y);
		Vector2[] directions = new Vector2[]{new Vector2(-1,0),new Vector2(1,0), new Vector2(0,-1), new Vector2(0,1)};
		Debug.Log("start:" + pos);
		//Get the map if it is undeclared
		if(m == null){
			m = GameObject.FindObjectOfType<Map>();
		}

		byte adj = m.getTileAdjacency((int)location.x,(int)location.y);
		//Clockwise from top, top,right,down,left
		bool [] passability = new bool[]{(adj >> 8 & 1) == 1, (adj >> 4 & 1) == 1, (adj >> 2 & 1) == 1, (adj & 1) == 1};

		if(passability[0]){
			neighbors.Add (pos + directions[0]);Debug.Log("neighbor:" + neighbors[neighbors.Count-1]);
		}
		if(passability[1]){
			neighbors.Add (pos + directions[1]);Debug.Log("neighbor:" + neighbors[neighbors.Count-1]);
		}
		if(passability[2]){
			neighbors.Add (pos + directions[2]);Debug.Log("neighbor:" + neighbors[neighbors.Count-1]);
		}
		if(passability[3]){
			neighbors.Add (pos + directions[3]);Debug.Log("neighbor:" + neighbors[neighbors.Count-1]);
		}

		return neighbors.ToArray();
	}
}
