using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Route {

	public int length;
	public List<ParentedNode> nodes;
	public List<Vector3> locations;

	public Route(){
		length = 0;
		locations = new List<Vector3>();
		nodes = new List<ParentedNode>();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Route"/> class.
	/// </summary>
	/// <param name="p">The parented node to build the route from.</param>
	public Route(ParentedNode p){
		length = 0;
		locations = new List<Vector3>();
		nodes = new List<ParentedNode>();
		ParentedNode n = p;
		while(n.parent != null){
			locations.Insert(0,n.location);
			nodes.Insert(0,n);
			Debug.Log (n.type);
			n = n.parent;
		}
		length = locations.Count;
	}

	public Route(Route r){
		this.length = r.length;
		this.locations = new List<Vector3>(r.locations);
	}

	public Route branchRoute(Vector3 v){
		Route r = new Route(this);
		r.locations.Add (v);
		return r;
	}

	public override string ToString(){
		StringWriter sw = new StringWriter();
		foreach(Vector3 n in locations){
			sw.Write(n.ToString());
		}
		return sw.ToString();
	}

	public void reverseRoute(){
		locations.Reverse();
	}

	public void append(Route r){
		if(r != null){
			this.locations.AddRange (r.locations);
			this.length += r.length;
		}

	}
	public void centerLocations(){
		for(int i = 0; i < locations.Count; i++){
			locations[i].Set(locations[i].x + .5f,locations[i].y,locations[i].z);
		}
	}
}
