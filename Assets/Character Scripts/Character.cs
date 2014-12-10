using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CharacterController))]
public class Character : AnimatedEntity {

	//For pathing:
	[HideInInspector]
	public Vector2 position;
	[HideInInspector]
	public IVector2 destination;
	protected int currentPathIndex;
	public Route path;

	//Standard Character variables.
	public float moveSpeed;
	public float maxHealth;
	public float currentHealth;

	public float hitForce;

	public float size = 1f;
	public float jumpSpeed = 10;
	public float gravity = 9.81f;
	public float landingDelay = 5;
	public float digTime = 0.25f;
	protected float digTimer = 0;

	public float weight;

	protected UnitManager um;
	protected EnemyManager em;

	public bool selected;

	[HideInInspector]
	public CharacterController cc;
	protected Vector2 currentMovement;
	protected Vector2 knockback;
	Vector2 lastPosition;

	public enum movementState {WALKING,JUMPING,CLIMBING,IDLE,DIGGING};
	public movementState currentState;
	public bool digging = false;

	protected Map map;

	public ParticleSystem particles;

	public bool rePath;

	public bool canLadder;
	public bool canMine;
	public int pathsPerFrame = 180;

	static int LADDERINDEX;

	protected bool pathing = true;
	protected bool moving = true;

	// Use this for initialization
	protected void Start () {
		//Here be dragons
		um = GameObject.FindObjectOfType<UnitManager>();
		em = GameObject.FindObjectOfType<EnemyManager>();

		LADDERINDEX = TileSpecList.getTileSpecInt("Ladder");

		currentMovement = new Vector2(0,0);

		map = GameObject.FindObjectOfType<Map>();

		currentState = movementState.IDLE;

		//Start the pathing coroutine
		StartCoroutine(getPath());
		StartCoroutine(computeState());
		cc = GetComponent<CharacterController>();
		position = new Vector2(Mathf.RoundToInt(this.transform.position.x),Mathf.RoundToInt(this.transform.position.y));

		selected = false;
		this.transform.localScale = new Vector3(size,size,size);

		this.right = Random.value>.5f;
	}

	private static Vector3 off = new Vector3(0.5f, 0.5f, -5);
	// Update is called once per frame
	new void FixedUpdate () {
		base.FixedUpdate();
		move ();
		if (path != null && currentPathIndex < path.length) {
			Vector2 s = transform.position+off;
			Vector2 d = path.locations[currentPathIndex]+off;
			Debug.DrawLine(s, d, destColor(d));
			for(int i = currentPathIndex; i < path.length - 1; i++){
				Vector2 src = path.locations[i]+off;
				Vector2 dst = path.locations[i+1]+off;
				Debug.DrawLine(src, dst, destColor(dst));
			}
		}
	}

	public Color destColor(Vector2 dest){
		if (map.isForegroundSolid(dest))
			return Color.cyan;
		if (!map.unnavigable(dest))
			return Color.yellow;
		if (map.ladderable(dest))
			return Color.green;
		return Color.black;
	}

	public void findPath(Vector2 v){
		position = new Vector2(Mathf.RoundToInt(this.transform.position.x),Mathf.RoundToInt(this.transform.position.y));
		destination = new IVector2(v.x,v.y);
		rePath = true;
	}

	//Sets the route for this character to follow
	public void setPath(Route r){
		path = r;
		currentPathIndex = 0;
		r.centerLocations();
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
			if(lastDest == destination && !rePath){
				pathing = false;
				waitTime = Mathf.Clamp(waitTime + .01f,.01f,.5f);
				//...Go back to start of the loop.
				goto reset;
			}
			pathing = true;
			rePath = false;

			if(!TileSpecList.getTileSpec(map.getByte(destination,Map.FOREGROUND_ID)).diggable){
				lastDest = destination;
				path = new Route();
				goto reset;
			}
			waitTime = .1f;
			Vector2 start = position;
			Vector2 end = new IVector2(destination.x ,destination.y);

			//Create a list of leaves
			List<ParentedNode> leaves = new List<ParentedNode>();

			//Create a list of branches (used leaves)
			List<ParentedNode> branches = new List<ParentedNode>();

			//If the character has to dig
			Route digRoute = new Route();

			//Add current position to the leaves
			leaves.Add (new ParentedNode(null,position,0));
			int count = 0;

			//While there are still leaves, and the destination hasn't changed.
			while(leaves.Count > 0){

				ParentedNode current = getSmallestLeaf(leaves);

				leaves.Remove(current);
				branches.Add(current);

				//If it found the path...
				if(current.location == end){
					//...Create a new route based on that last node.
					//ParentedNode p = new ParentedNode(current.parent,end,0);
					Route r = new Route(current);
					setPath(r);
					if(r.locations.Count > 0){
						lastDest = new IVector2(r.locations[r.locations.Count-1].x,r.locations[r.locations.Count-1].y);
					}
					//Break out
					goto reset;
				}

				//Add new leaves, both open neighbors and ones where you dig.
				addToLeaves(current,current.GetNeighbors(),branches,leaves,start,end,0,ParentedNode.Type.WALK);
				if(canMine)
					addToLeaves(current,current.GetDigNeighbors(),branches,leaves,start,end,1,ParentedNode.Type.DIG);
				if(canLadder)
					addToLeaves(current,current.GetLadderNeighbors(),branches,leaves,start,end,3,ParentedNode.Type.LADDER);

				count ++;

				if(rePath){
					goto reset;
				}
				//Only do 20 cycles per frame
				if(count % pathsPerFrame == 0){
					yield return new WaitForSeconds(1);
					if(count > 10000){
						destination = lastDest;
						goto reset;
					}
				}
			}

			//If it goes through and cant find anything, 
			Debug.Log ("Path not found");
			waitTime += .01f;

		}
	}

	//Gets the smallest node in leaves
	private ParentedNode getSmallestLeaf(List<ParentedNode> leaves){
		//Create a parented node
		ParentedNode current = new ParentedNode(null,Vector2.zero,float.MaxValue);
		current.weight = float.MaxValue;
		//Check to find the lowest weighted leaf
		foreach(ParentedNode p in leaves){	
			if(p.weight < current.weight){
				current = p;
			}				                             
		}
		return current;
	}

	//Add the specified nodes as leaves, ensuring that they are parented to current and not in branches or leaves.
	private void addToLeaves(ParentedNode current, Vector2[] positions, List<ParentedNode> branches,
	                        List<ParentedNode> leaves, Vector2 start, Vector2 end, int addedWeight, ParentedNode.Type t){
		//Get the neighbors and add them to leaves, parented to the current node.
		foreach(Vector2 v in positions){
			//Don't add it if its already in the leaves or branches.
			if(!ContainsNode(leaves,branches,v)){

				ParentedNode p = new ParentedNode(current,v,hueristic(v,end) + addedWeight);
				p.type = t;
				leaves.Add(p);
			}
		}
	}
	
	//Checks if the vector is in either list.
	static bool ContainsNode(List<ParentedNode> l,List<ParentedNode>b, Vector2 n){
		List<ParentedNode> combined = new List<ParentedNode>();
		combined.AddRange(l);
		combined.AddRange(b);
		//Check l
		foreach(ParentedNode p in combined){
			if((p.location - n).magnitude < 1){
				return true;
			}
		}
		return false;
	}

	//Provides a hueristic weight based on the distance from the destination.
	public float hueristic(Vector3 n, Vector3 destination){
		int d = map.getByte((Vector2)n,Map.DURABILITY);
		float xdif = n.x - destination.x;
		float ydif = n.y - destination.y;
		return (Mathf.Abs(xdif)+Mathf.Abs(ydif)) + d;
	}

	//Moves this character along its route.
	public virtual void move(){
		if(path != null && currentPathIndex < path.locations.Count){

			moving = true;

			Vector2 currentpos = new Vector2(this.transform.position.x + size/2,this.transform.position.y);
			Vector2 dest = path.locations[currentPathIndex];
			ParentedNode node = path.nodes[currentPathIndex];
			Vector2 v = dest - (Vector2)this.transform.position;

			if(v.magnitude > 1.5f)
				rePath = true;

			bool shouldDig = map.isForegroundSolid(dest);
			bool ladder = !shouldDig && map.ladderable(dest);

			switch(node.type){
			case ParentedNode.Type.WALK:
				walkcase(currentpos,dest,node,v);
				break;
			case ParentedNode.Type.DIG:
				digcase(currentpos,dest,node,v);
				break;
			case ParentedNode.Type.LADDER:
				laddercase(currentpos,dest,node,v);
				break;
			}

			if (shouldDig){
				Vector3 a = (Vector3)currentpos + new Vector3(0.2f, 0.7f, -5);
				Vector3 b = (Vector3)currentpos + new Vector3(0.2f, 0.3f, -5);
				Vector3 c = (Vector3)currentpos + new Vector3(-0.2f, 0.3f, -5);
				Vector3 d = (Vector3)currentpos + new Vector3(-0.2f, 0.7f, -5);

				Color col = Color.cyan;

				Debug.DrawLine(a, b, col);
				Debug.DrawLine(b, c, col);
				Debug.DrawLine(c, d, col);
				Debug.DrawLine(d, a, col);
			}

			currentMovement.x = ((Mathf.Abs(v.x) < .05)?0:Mathf.Sign(v.normalized.x) * moveSpeed);

			if(v.magnitude < .25f){
				position = path.locations[currentPathIndex];
				currentPathIndex++;
			}
			lastPosition = currentpos;
			//transform.Translate(v.normalized * moveSpeed* Time.deltaTime);
		}
		else{
			moving = false;
			currentMovement.x = 0;
		}
		applyGravity();
		cc.Move(currentMovement * Time.fixedDeltaTime);

	}

	//****************************************************************************//

	void walkcase(IVector2 currentpos,IVector2 dest,ParentedNode node,Vector2 v){
		if((v.y > .25f && Mathf.Abs(v.x) < .1f) || map.getForeground(currentpos) == TileSpecList.getTileSpec(LADDERINDEX) && canLadder){
			laddercase(currentpos,dest,node,v);
			return;
		}
		currentState = movementState.WALKING;
		if(v.y > .25f && (Mathf.Abs(v.x) > .1f && cc.velocity.x == 0)){// || (lastPosition - currentpos).magnitude == 0){
			jump ();
			currentState = movementState.JUMPING;
		}
	}
	void digcase(IVector2 currentpos,IVector2 dest,ParentedNode node,Vector2 v){
		handleDigging(dest,v);
	}
	void laddercase(IVector2 currentpos,IVector2 dest,ParentedNode node,Vector2 v){
		bool s = map.getForeground(currentpos) == TileSpecList.getTileSpec("Ladder");
		bool f = map.getForeground(currentpos) == TileSpecList.getTileSpec("Ladder");
		if(currentpos.x == dest.x){
			ladderTile(currentpos);
			ladderTile(dest);
		}
		currentMovement.y = Mathf.Sign(v.y) * moveSpeed;
	}

	void ladderTile(IVector2 t){
		if(!canLadder)
			return;
		currentState = movementState.CLIMBING;
		TileSpec ts = map.getForeground(t);
		if(!ts.solid && ts != TileSpecList.getTileSpec(LADDERINDEX)){
			map.setTile(t,(byte)LADDERINDEX,(byte)1);
		}
	}

	//****************************************************************************//

	void applyGravity(){
		if(currentState == movementState.CLIMBING){
			return;
		}
		if(!cc.isGrounded || currentMovement.y > 1){
			currentMovement.y -= gravity * Time.fixedDeltaTime;
		}
		else{
			currentMovement.y = 0;
		}
	}


	//Handles the character digging.
	public void handleDigging(Vector2 dest,Vector2 v){
		if(digging){
			this.currentState = movementState.DIGGING;
			if(v.y < -.25){
				jump(.75f);
				emitParticles();
			}
			//If it is done digging
			if((digTimer -= Time.fixedDeltaTime) <= 0){
				mine (dest);
			}
		}
		//Detect if it needs to start digging
		else if(map.isForegroundSolid(dest)){
			digTimer = digTime;
			digging = true;
		}
	}

	protected virtual void mine(IVector2 v){
		if(!canMine)
			return;
		byte d = map.getByte (v, Map.DURABILITY);
		if (d == 0){
			digging = false;
			TileSpec ts = TileSpecList.getTileSpec(map.getByte(v,Map.FOREGROUND_ID));
			InventroyManager.instance.addToInventory(ts.resource);
			map.setTile(v,0,map.getByte(v,Map.BACKGROUND_ID));
		}
		else{
			map.setByte(v, Map.DURABILITY, (byte)(d - 1));
			digTimer = digTime;
		}
		// This element type should be determined by the element being mined

	}

	//Determines the current movement state and what it should transition to
	protected IEnumerator computeState(){

		if(digging){
			currentState = movementState.DIGGING;
		}

		int counter = 0;
		while(true){
			switch(currentState){
			case movementState.IDLE:
				animater.animationID = 2;
				if(currentMovement.x != 0){
					currentState = movementState.WALKING;
				}
				break;
			case movementState.JUMPING:
				if(cc.isGrounded){
					currentState = movementState.WALKING;
					counter = (int)landingDelay;
				}
				break;
			case movementState.CLIMBING:
				//TODO Set animation
				break;
			case movementState.WALKING:
				animater.animationID = 1;
				if(currentMovement.x == 0){
					currentState = movementState.IDLE;
				}
				break;
			case movementState.DIGGING:
				animater.animationID = 0;
				emitParticles();
				if(!digging){
					currentState = movementState.IDLE;
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

	//Jump a modified height
	public virtual void jump(float mod){
		if(cc.isGrounded){
			currentMovement.y = jumpSpeed * mod;
		}
	}

	//Jump normally
	public virtual void jump(){
		if(cc.isGrounded){
			currentMovement.y = jumpSpeed * Util.randomSpread();
		}
	}

	//Damagae and apply a force to the character
	public void hit(float f,float force,Transform source){
		Vector2 dir = source.position - this.transform.position;
		knockback = dir.normalized * force;
		this.path = null;
		this.currentHealth -= f;
	}

	public void changeSelection (bool b)
	{
		this.selected = b;
	}

	public void emitParticles(){
		if(particles){
			particles.Emit(1);
		}
	}
}
