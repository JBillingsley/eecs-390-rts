using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Author: Henry Eastman
//Controls the player, including inputs and movement.
[RequireComponent(typeof(CharacterController))]
public class Player : Character {
	
	public static Player singleton;

	public float jumpSpeed = 100;
	public float gravity = 9.81f;

	public LayerMask terrainLayer;
	public LayerMask itemLayer;

	private bool drawSelection;
	public Rect selectionBox;
	
	public Texture selectBox;

	CharacterController cc;

	Vector2 movement = new Vector2();

	//The current units
	public List<Character> selected; /* Will be multiple units */
	private List<Character> units;

	//Inputs
	private bool selectInputDown = false;
	private bool selectInput = false;
	private bool selectInputUp = false;
	private bool moveInput = false;
	private bool addSelect = false;

	//public Item equipped;

	//private NavigationNode lastPosition;

	// Use this for initialization
	void Start () {
		cc = this.GetComponent<CharacterController>();
		singleton = this;
		position = new Vector2((int)this.transform.position.x,(int)this.transform.position.y);
		selectionBox = new Rect(0,0,0,0);
		drawSelection = false;
		units = new List<Character>( GameObject.FindObjectsOfType<Unit>());
		//position = null;
	}

	void Update(){
		getInputs();
		if(selectInputDown){
			startBox();
		}
		if(selectInput){
			continueBox();
		}
		else{
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

	void startBox() {
		selectionBox = new Rect();
		drawSelection = true;
		
		Vector2 pos = Input.mousePosition;
		selectionBox.x = pos.x;
		selectionBox.y = Screen.height - pos.y;
	}
	//End the selection box;
	void continueBox(){
		Vector2 pos = Input.mousePosition;
		selectionBox.width = pos.x - selectionBox.x;
		selectionBox.height = (Screen.height - pos.y) - selectionBox.y;
	}
	
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
		foreach(Character u in units){
			if(u){
				Vector2 v = Camera.main.WorldToScreenPoint(u.transform.position);
				if(selectionBox.Contains(new Vector2(v.x,Screen.height - v.y))){
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
		RaycastHit hit;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(!addSelect){
			foreach(Character u in selected){
				if(u){
					//Select the units
					//u.changeSelection(false);
				}
			}
			selected = new List<Character>();
		}

		//Select units by pressing them.

		/*
		if(Physics.Raycast(r,out hit,100f,myUnits.value)){

			addToSelection(hit.collider.GetComponent<Unit>());
			return;
		}*/
	}
	
	//Move the units to the specified place. (Mouse position)
	void moveUnits() {

		Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//StartCoroutine("findPath",v);
		foreach(Character c in selected){
			c.StartCoroutine("findPath",v);
		}
	}

	//Add the unit to the selection list
	void addToSelection(Character u){
		selected.Add(u);
		//Select the unit
		//u.changeSelection(true);
	}
	
	//Remove the unit from the selection list
	void removeFromSelection(Character u){
		selected.Remove(u);
		//unselect the unit
		//u.changeSelection(false);
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
		movement.x /= 2f;
		movement.x = Mathf.Clamp(movement.x + Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime,-1,1);
		if(cc.isGrounded){
			movement.y = 0;
			if(Input.GetAxis("Jump") > .5f){
				movement.y = jumpSpeed;
			}
		}
		if(!cc.isGrounded){
			movement.y -= gravity * Time.fixedDeltaTime;
		}
		Vector2 mov = transform.TransformDirection(movement);
		mov *= moveSpeed;
		cc.Move(mov * Time.deltaTime);

	}
}
