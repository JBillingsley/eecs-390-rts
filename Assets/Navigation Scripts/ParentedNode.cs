using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParentedNode {

	public static Map m;
	public ParentedNode parent;
	//public NavigationNode node;
	public Vector3 location;
	public float weight;

	public ParentedNode(ParentedNode parent,Vector3 node, float weight){
		this.parent = parent;
		this.weight = weight;
		if(parent != null){
			this.weight += parent.weight;
		}
		//this.node = node;
	}

	public override string ToString ()
	{
		//string s = node.transform.position + " " + weight;
		return ""; //s
	}

	public Vector3[] GetNeighbors(){
		List<Vector3> neighbors = new List<Vector3>();
		Vector2[] directions = new Vector2[]{new Vector2(-1,0),new Vector2(1,0), new Vector2(0,-1), new Vector2(0,1)};

		//Get the map if it is undeclared
		if(m == null){
			m = GameObject.FindObjectOfType<Map>();
		}
		Tile start = m.getTile((int)location.x,(int)location.y);


		return neighbors.ToArray();
	}
}
