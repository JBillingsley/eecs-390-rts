using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Author: Henry Eastman
//Controls the player, including inputs and movement.

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Player : AnimatedEntity {
	
	public static Player singleton;
	
	public LayerMask terrainLayer;
	public LayerMask itemLayer;

	private bool drawSelection;
	public Rect selectionBox;
	
	public Texture selectBox;

	//The current units
	public List<Character> selected; /* Will be multiple units */

	//Inputs
	private bool selectInputDown = false;
	private bool selectInput = false;
	private bool selectInputUp = false;
	private bool moveInput = false;
	private bool addSelect = false;

	private UnitManager um;

	public float moveSpeed = 2;
	public float jumpSpeed = 2;
	public float gravity = 1;
	private bool onGround = false;
	private Transform platform;
	private bool stuck = false;

	
	Vector2 currentMovement = new Vector2();

	
	public enum movementState {WALKING,JUMPING,LANDING,IDLE,DIGGING};
	public movementState currentState;
	public bool digging = false;

	// Use this for initialization
	protected void Start () {
		singleton = this;
		selectionBox = new Rect(0,0,0,0);
		drawSelection = false;
		um = GameObject.FindObjectOfType<UnitManager>();
		StartCoroutine(computeState());
	}

	void Update(){
		getInputs();
		if(selectInputDown){
			startBox();
		}
		if(selectInput){
			continueBox();
		}
		if(selectInputUp){
			endBox();
		}
		if(moveInput){
			moveUnits();
		}
	}

	// Update is called once per frame
	public new void FixedUpdate () {
		base.FixedUpdate();
		move();
	}

	void OnGUI(){
		if(drawSelection){
			Debug.Log("drawing");
			Rect r = selectionBox;
			if (r != null){
				r.x = Screen.height - r.x;
				GUI.DrawTexture(selectionBox,selectBox);
			}
		}
	}

	//Gets currently needed inputs.
	void getInputs () { //*********** WILL WANT TO USE AXES ***********//
		selectInput = Input.GetMouseButton(0);
		selectInputDown = Input.GetMouseButtonDown(0);
		selectInputUp = Input.GetMouseButtonUp(0);
		moveInput = Input.GetMouseButtonDown(1);
		addSelect = Input.GetKey(KeyCode.LeftShift); 
	}

	//Starts the selection box
	void startBox() {
		selectionBox = new Rect();
		drawSelection = true;
		
		Vector2 pos = Input.mousePosition;
		selectionBox.x = pos.x;
		selectionBox.y = Screen.height - pos.y;
	}

	//continue the selection box;
	void continueBox(){
		Vector2 pos = Input.mousePosition;
		selectionBox.width = pos.x - selectionBox.x;
		selectionBox.height = (Screen.height - pos.y) - selectionBox.y;
	}

	//End the selection box.
	void endBox(){
		selectionBox = standardizeRect(selectionBox);
		
		drawSelection = false;
		
		//If the box is too small for box select;
		if(selectionBox.width < .1 && selectionBox.height < .1){
			selectUnits();
			return;
		}
		if(!addSelect){
			foreach(Character u in selected){
				//Make changes to the unit for being unselected
				//u.changeSelection(false);
			}
			selected = new List<Character>();
		}
		foreach(Character u in um.units){
			if(u){
				Vector2 v = Camera.main.WorldToScreenPoint(u.transform.position);
				Vector2 w = Camera.main.WorldToScreenPoint(u.transform.position + u.transform.localScale);

				Vector2 pos = (v+w)/2;

				if(selectionBox.Contains(new Vector2(pos.x,Screen.height - pos.y))){
					addToSelection(u);
				}
			}
		}
	}
	
	static Rect standardizeRect(Rect r){
		if(r.width < 0){
			r.x = r.x + r.width;
			r.width = - r.width;
		}
		if(r.height < 0){
			r.y = r.y + r.height;
			r.height = - r.height;
		}
		return r;
	}
	
	//Select units if you currently have mouse input.
	void selectUnits() {
		Vector2 mousePos = Input.mousePosition;
		if(!addSelect){
			foreach(Character u in selected){
				if(u){

					//unSelect the units
					u.changeSelection(false);
				}
			}
			selected = new List<Character>();
		}

		//Select units by pressing them.

		foreach(Character u in um.units){
			
			Vector2 v = Camera.main.WorldToScreenPoint(u.transform.position);
			Vector2 w = Camera.main.WorldToScreenPoint(u.transform.position + u.transform.localScale);
			if(new Rect(v.x,v.y,(w-v).x,(w-v).y).Contains(mousePos)){
				selected.Add (u);
				return;
			}
		}
	}
	
	//Move the units to the specified place. (Mouse position)
	void moveUnits() {

		Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//StartCoroutine("findPath",v);
		foreach(Character c in selected){
			c.findPath(v);
		}
	}

	//Add the unit to the selection list
	void addToSelection(Character u){
		selected.Add(u);
		Debug.Log ("adding unit");
		//Select the unit
		u.changeSelection(true);
	}
	
	//Remove the unit from the selection list
	void removeFromSelection(Character u){
		selected.Remove(u);
		//unselect the unit
		u.changeSelection(false);
	}

	void makeRoute(){
		//Route r = this.position.getRoute(position.goal);
		//this.setPath(r);
	}

	RaycastHit2D getCollider(LayerMask l){
		RaycastHit2D hit;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		return (hit = Physics2D.GetRayIntersection(r,100f,l.value));
	}

	public void move(){
		currentMovement.x = Mathf.Clamp(Input.GetAxis("Horizontal") * moveSpeed,-moveSpeed,moveSpeed);
		if(onGround || stuck){
			currentMovement.y = 0;
			if(Input.GetAxis("Jump") > .5f){
				currentMovement.y = jumpSpeed;
			}
		}
		else{
			currentMovement.y -= gravity * Time.fixedDeltaTime;
		}
		Debug.Log (currentMovement);
		this.rigidbody.AddForce(currentMovement);
	}

	//Determines the current movement state and what it should transition to
	protected IEnumerator computeState(){
		
		if(digging){
			currentState = movementState.DIGGING;
			animater.animationID = 0;
		}
		
		int counter = 0;
		while(true){
			switch(currentState){
			case movementState.IDLE:
				if(currentMovement.x != 0){
					currentState = movementState.WALKING;
					animater.animationID = 1;
				}
				break;
			case movementState.JUMPING:
				if(onGround){
					currentState = movementState.WALKING;
					animater.animationID = 1;
				}
				break;
			case movementState.LANDING:
				counter --;
				if(counter == 0){
					currentState = movementState.WALKING;
					animater.animationID = 1;
				}
				break;
			case movementState.WALKING:
				if(!onGround && currentMovement.y != 0){
					currentState = movementState.JUMPING;
				}
				else{
					if(currentMovement.x == 0){
						currentState = movementState.IDLE;
						animater.animationID = 2;
					}
				}
				break;
			case movementState.DIGGING:
				animater.animationID = 0;
				if(!digging){
					currentState = movementState.IDLE;
					animater.animationID = 1;
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

	public void OnCollisionStay(Collision col){
		Vector2 diff = col.contacts[0].point - this.transform.position;
		Vector2 normal = col.contacts[0].normal;
		if(col.gameObject.tag == "Unit" && stuck == false){
			this.transform.parent = col.gameObject.transform;
			platform = col.gameObject.transform;
			stuck = true;
			this.currentMovement = Vector3.zero;
		}
		else{
			onGround = false;
			foreach(ContactPoint p in col.contacts){
				if(Mathf.Abs(p.normal.x) < .01f){
					onGround = true;
				}
			}
		}
	}

	public void OnCollisionExit(Collision col){
		if(col.gameObject.tag == "Unit" && col.gameObject.transform == platform){
			this.transform.parent = null;
			platform = null;
			stuck = false;
		}
		else{
			onGround = false;
		}
	}
}
