using UnityEngine;
using System.Collections;

public class ParentedNode {

	public ParentedNode parent;
	//public NavigationNode node;
	public Vector3 location;
	public float weight;

	/*public ParentedNode(ParentedNode parent,Vector3 node, float weight){
		this.parent = parent;
		this.weight = weight;
		if(parent != null){
			this.weight += parent.weight;
		}
		//this.node = node;
	}*/

	public override string ToString ()
	{
		//string s = node.transform.position + " " + weight;
		return ""; //s
	}
}
