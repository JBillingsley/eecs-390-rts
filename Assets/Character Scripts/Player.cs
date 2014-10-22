using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Author: Henry Eastman
//Controls the player, including inputs and movement.

public class Player : Character {
	
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

	//public Item equipped;

	//private NavigationNode lastPosition;

	// Use this for initialization
	protected void Start () {
		base.Start();
		cc = this.GetComponent<CharacterController>();
		singleton = this;
		position = new Vector2((int)this.transform.position.x,(int)this.transform.position.y);
		selectionBox = new Rect(0,0,0,0);
		drawSelection = false;
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
	public void FixedUpdate () {
		base.FixedUpdate();
		move();
	}

	void OnGUI(){
		if(drawSelection){
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

	public override void move(){
		currentMovement.x = 0;
		currentMovement.x = Mathf.Clamp(Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime,-1,1);
		if(cc.isGrounded){
			currentMovement.y = 0;
			if(Input.GetAxis("Jump") > .5f){
				currentMovement.y = jumpSpeed;
			}
		}
		if(!cc.isGrounded){
			currentMovement.y -= gravity * Time.fixedDeltaTime;
		}
		Vector2 mov = transform.TransformDirection(currentMovement);
		mov *= moveSpeed;
		cc.Move(mov * Time.deltaTime);

	}
}
