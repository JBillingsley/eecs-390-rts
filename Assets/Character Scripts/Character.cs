using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public Vector2 position;

	//public Vector3 destination;
	protected int currentPathIndex;
	protected Route path;

	public float moveSpeed;
	public float maxHealth;
	public float currentHealth;

	// Use this for initialization
	void Start () {

		//position = GameObject.FindObjectOfType<NavigationNode>();
		//transform.position = position.transform.position;
		//position.open = true;
	}
	
	// Update is called once per frame
	void Update () {
		move ();
	}

	//Sets the route for this character to follow
	public void setPath(Route r){
		path = r;
		//destination = r.nodes[r.nodes.Count-1];
		currentPathIndex = 0;
		/*for(int i = 1; i < path.locations.Count-1; i++){
			Vector3 p = path.locations[i];
			Vector3 p1 = path.locations[i-1];
			Vector3 p2 = path.locations[i+1];

			Vector3 nCenter = (p1+p2)/2;
			path.locations[i] = (p + nCenter)/2;
		}*/
	}

	public Route getPath(Vector2 start, Vector2 end){

		//Create a list of leaves
		List<ParentedNode> leaves = new List<ParentedNode>();

		//Create a list of branches (used leaves)
		List<ParentedNode> branches = new List<ParentedNode>();

		//Add current position to the leaves
		leaves.Add (new ParentedNode(null,position,0));

		int count = 0;
		//While there are still leaves
		while(leaves.Count > 0 && count < 100){

			//Create a parented node
			ParentedNode current = new ParentedNode(null,new Vector2(0,0),float.MaxValue);
			bool flag = false;
			//Check to find the lowest weighted leaf
			foreach(ParentedNode p in leaves){
				if(p.weight < current.weight){
					current = p;
					flag = true;
				}				                             
			}
			//if one wasnt selected
			if(!flag){
				return null;
			}

			leaves.Remove(current);
			branches.Add(current);
			if(current.location == end){
				return new Route(current);
			}
			foreach(Vector3 v in current.GetNeighbors()){

				if(!ContainsNode(leaves,branches,v)){
					leaves.Add(new ParentedNode(current,new Vector2((int)v.x,(int)v.y),hueristic(v,end)));
				}
			}
			count ++;
		}
		return null;
	}

	static bool ContainsNode(List<ParentedNode> l,List<ParentedNode>b, Vector2 n){
		//Check l
		foreach(ParentedNode p in l){
			if((p.location - n).magnitude < 1){
				return true;
			}
		}
		//check b
		foreach(ParentedNode a in b){
			if((a.location - n).magnitude < 1){
				return true;
			}
		}
		return false;
	}

	public static float hueristic(Vector3 n, Vector3 destination){
		float xdif = n.x - destination.x;
		float ydif = n.y - destination.y;
		return (Mathf.Abs(xdif)+Mathf.Abs(ydif));
	}

	//Moves this character along its route.
	protected void move(){
		if(path != null){
			if(currentPathIndex >= path.length){
				return;
			}

			Vector3 v = path.locations[currentPathIndex] - this.transform.position;
			if(v.magnitude < .1){
				position = path.locations[currentPathIndex];
				currentPathIndex++;
			}
			transform.Translate(v.normalized * moveSpeed * Time.deltaTime);
		}
	}
}
