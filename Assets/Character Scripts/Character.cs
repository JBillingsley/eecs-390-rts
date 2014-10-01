using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	//For pathing:
	[HideInInspector]
	public Vector2 position;
	[HideInInspector]
	public Vector2 destination;
	protected int currentPathIndex;
	protected Route path;

	//Standard Character variables.
	public float moveSpeed;
	public float maxHealth;
	public float currentHealth;

	// Use this for initialization
	void Start () {
		//Start the pathing coroutine
		StartCoroutine(getPath());
		position = new Vector2((int)this.transform.position.x,(int)this.transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		move ();
	}

	protected void findPath(Vector2 v){
		destination = new IVector2(v.x,v.y);
	}

	//Sets the route for this character to follow
	public void setPath(Route r){
		path = r;
		currentPathIndex = 0;

	}

	//Coroutine which calculates the path to the destination.
	public IEnumerator getPath(){
		Vector2 lastDest = position;

	reset:
		while(true){
			//This routine happens every second.
			yield return new WaitForSeconds(1);

			//If the destination hasn't changed...
			if(lastDest == destination){
				//...Go back to start of the loop.
				continue;
			}
			Vector2 start = position;
			Vector2 end = destination;
			Debug.Log ("Trying to path from " + start + " to " + end + ".");

			//Create a list of leaves
			List<ParentedNode> leaves = new List<ParentedNode>();

			//Create a list of branches (used leaves)
			List<ParentedNode> branches = new List<ParentedNode>();

			//Add current position to the leaves
			leaves.Add (new ParentedNode(null,position,0));
			int count = 0;
			ParentedNode current = new ParentedNode(null,start,float.MaxValue);

			//While there are still leaves, and the destination hasn't changed.
			while(leaves.Count > 0 && end.Equals(destination)){
				//Create a parented node
				current.weight = float.MaxValue;
				//Check to find the lowest weighted leaf
				foreach(ParentedNode p in leaves){	
					if(p.weight < current.weight){
						current = p;
					}				                             
				}

				leaves.Remove(current);
				branches.Add(current);

				//If it found the path...
				if(current.location == end){
					//...Create a new route based on that last node.
					setPath(new Route(current));
					lastDest = end;
					//Break out of this while loop.
					goto reset;
				}

				//Get the neighbors and add them to leaves, parented to the current node.
				foreach(Vector2 v in current.GetNeighbors()){
					///Dont add it if it was already dealt with.
					if(!ContainsNode(leaves,branches,v)){
						leaves.Add(new ParentedNode(current,v,hueristic(v,start) + hueristic(v,end)));
					}
				}

				//Only do 20 cycles per frame
				if(count % 20 == 0){
					yield return null;
				}
			}

			//If it goes through and cant find anything, 
			Debug.Log ("Path not found");

		}
	}

	//Checks if the vector is in either list.
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

	//Provides a hueristic weight based on the distance from the destination.
	public static float hueristic(Vector3 n, Vector3 destination){
		float xdif = n.x - destination.x;
		float ydif = n.y - destination.y;
		return (Mathf.Abs(xdif)+Mathf.Abs(ydif));
	}

	//Moves this character along its route.
	public virtual void move(){
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
