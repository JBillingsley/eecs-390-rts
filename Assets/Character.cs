using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {


	public Vector3 position;

	//public Vector3 destination;
	public int currentPathIndex;
	public Route path;

	public float moveSpeed;

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
		for(int i = 1; i < path.locations.Count-1; i++){
			Vector3 p = path.locations[i];
			Vector3 p1 = path.locations[i-1];
			Vector3 p2 = path.locations[i+1];

			Vector3 nCenter = (p1+p2)/2;
			path.locations[i] = (p + nCenter)/2;
		}
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
