using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterController))]
public class Character : AnimatedEntity {

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

	public float jumpSpeed = 10;
	public float gravity = 9.81f;
	public float landingDelay = 5;

	[HideInInspector]
	public CharacterController cc;
	protected Vector2 currentMovement;
	Vector2 lastPosition;

	public enum movementState {WALKING,JUMPING,LANDING,IDLE};
	protected movementState currentState;

	protected Map map;

	// Use this for initialization
	protected void Start () {
		currentMovement = new Vector2(0,0);

		map = GameObject.FindObjectOfType<Map>();

		currentState = movementState.IDLE;

		//Start the pathing coroutine
		StartCoroutine(getPath());
		StartCoroutine(computeState());
		cc = GetComponent<CharacterController>();
		position = new Vector2(Mathf.RoundToInt(this.transform.position.x),Mathf.RoundToInt(this.transform.position.y));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		base.FixedUpdate();
		move ();
	}

	public void findPath(Vector2 v){
		position = new Vector2(Mathf.RoundToInt(this.transform.position.x),Mathf.RoundToInt(this.transform.position.y));
		destination = new Vector2(v.x,v.y);
	}

	//Sets the route for this character to follow
	public void setPath(Route r){
		path = r;
		currentPathIndex = 0;

	}

	//Coroutine which calculates the path to the destination.
	public IEnumerator getPath(){
		Vector2 lastDest = position;
		float waitTime = .1f;

	reset:
		while(true){
			//This routine happens every second.
			yield return new WaitForSeconds(waitTime);

			//If the destination hasn't changed...
			if(lastDest == destination){
				waitTime = Mathf.Clamp(waitTime + .01f,.1f,1f);
				//...Go back to start of the loop.
				goto reset;
			}
			waitTime = .1f;
			Vector2 start = position;
			Vector2 end = new Vector2(destination.x ,Mathf.FloorToInt(destination.y));;
			Vector2 endTile = new IVector2(destination.x,destination.y);

			if(map.getForeground(endTile).solid){
				Debug.Log ("Setting air");
				map.setTile(endTile,0,0);
			}

			//Create a list of leaves
			List<ParentedNode> leaves = new List<ParentedNode>();

			//Create a list of branches (used leaves)
			List<ParentedNode> branches = new List<ParentedNode>();

			//Add current position to the leaves
			leaves.Add (new ParentedNode(null,position,0));
			int count = 0;
			ParentedNode current = new ParentedNode(null,start,float.MaxValue);

			//While there are still leaves, and the destination hasn't changed.
			while(leaves.Count > 0){
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
				if(current.location == endTile){
					//...Create a new route based on that last node.
					//ParentedNode p = new ParentedNode(current.parent,end,0);
					current.location.x += (1 - Util.randomSpread());
					Route r = new Route(current);
					setPath(r);
					lastDest = endTile;
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
				if(count % 40 == 0){
					yield return null;
				}
			}

			//If it goes through and cant find anything, 
			Debug.Log ("Path not found");
			waitTime += .01f;

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
		if(path != null && currentPathIndex < path.locations.Count){

			Vector2 currentpos = new Vector2(this.transform.position.x,this.transform.position.y);

			Vector3 dest = path.locations[currentPathIndex];

			Vector3 v = dest - this.transform.position;

			currentMovement.x = Mathf.Sign(v.normalized.x) * moveSpeed;

			if(v.y > .25f && (Mathf.Abs(v.x) > 0 && cc.velocity.x == 0)){// || (lastPosition - currentpos).magnitude == 0){
				jump ();
			}

			if(v.magnitude < .1){
				position = path.locations[currentPathIndex];
				currentPathIndex++;
			}
			lastPosition = currentpos;
			//transform.Translate(v.normalized * moveSpeed* Time.deltaTime);
		}
		else{
			currentMovement.x = 0;
		}
		if(!cc.isGrounded || currentMovement.y > 1){
			currentMovement.y -= gravity * Time.fixedDeltaTime;
		}
		else{
			currentMovement.y = 0;
		}
		cc.Move(currentMovement * Time.fixedDeltaTime);

	}

	public IEnumerator computeState(){
		int counter = 0;
		while(true){
			switch(currentState){
			case movementState.IDLE:
				if(currentMovement.x != 0){
					currentState = movementState.WALKING;
					animation.animationID = 1;
				}
				break;
			case movementState.JUMPING:
				if(cc.isGrounded){
					currentState = movementState.WALKING;
					animation.animationID = 1;
					counter = (int)landingDelay;
				}
				break;
			case movementState.LANDING:
				counter --;
				if(counter == 0){
					currentState = movementState.WALKING;
					animation.animationID = 1;
				}
				break;
			case movementState.WALKING:
				if(!cc.isGrounded && currentMovement.y != 0){
					currentState = movementState.JUMPING;
				}
				else{
					if(currentMovement.x == 0){
						currentState = movementState.IDLE;
						animation.animationID = 2;
					}
				}
				break;
			}
			if(currentMovement.x < 0){
				right = false;
			}
			if(currentMovement.x > 0){
				right = true;
			}
			yield return null;
		}
	}

	public virtual void jump(){
		if(cc.isGrounded){
			currentMovement.y = jumpSpeed * Util.randomSpread();
		}
	}

	public void hit(float f){
		this.currentHealth -= f;
	}
}
