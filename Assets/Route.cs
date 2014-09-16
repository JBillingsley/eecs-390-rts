using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Route {

	public int length;
	//public List<NavigationNode> nodes;
	public List<Vector3> locations;

	public Route(){
		length = 0;
		locations = new List<Vector3>();
	}

	public Route(ParentedNode p){
		length = 0;
		locations = new List<Vector3>();
		ParentedNode n = p;
		while(n.parent != null){
			locations.Insert(0,n.location);
			n = n.parent;
		}
		length = locations.Count;
	}

	public Route(Route r){
		this.length = r.length;
		this.locations = new List<Vector3>(r.locations);
	}

	/*public Route branchRoute(NavigationNode n){
		Route r = new Route(this);
		r.nodes.Add (n);
		r.locations.Add(n.transform.position);
		return r;
	}*/

	public override string ToString(){
		StringWriter sw = new StringWriter();
		foreach(Vector3 n in locations){
			sw.Write(n.ToString());
		}
		return sw.ToString();
	}
}
