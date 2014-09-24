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
		singleton = this;
		position = new Vector2((int)this.transform.position.x,(int)this.transform.position.y);
		selectionBox = new Rect(0,0,0,0);
		drawSelection = false;
		units = new List<Character>( GameObject.FindObjectsOfType<Character>());
		//position = null;
	}
	
	// Update is called once per frame
	void Update () {
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
		/*
		else if(Input.GetMouseButtonDown(1)){
			//Check what tile is under here.
			RaycastHit2D hit = new RaycastHit2D();
			if(hit = getCollider(terrainLayer)){
				Collider2D col = hit.collider;
				//NavigationNode node = col.GetComponent<NavigationNode>();
				Character c = this;
				if(node.open){
					position.goal = node;
					Invoke("makeRoute",0f);
				}
			}
		}*/
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
		moveInput = Input.GetMouseButton(1);
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

		Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		setPath(getPath(position,new Vector2((int)v.x,(int)v.y)));
		//Move the selected units to the desired position (where the mouse is)

		/*
		RaycastHit hit;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

		
		if(Physics.Raycast(r,out hit,100f,terrain.value)){
			foreach(Unit u in selected){
				u.changeDestination(hit.point);
			}
		}*/
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

	/*void pickupItem(RaycastHit2D hit){

		Item i = hit.collider.gameObject.GetComponent<Item>();
		i.rigidbody2D.isKinematic = true;
		equipped = i;
		i.transform.position = this.transform.position;
		i.transform.parent = this.transform;
	}*/

	RaycastHit2D getCollider(LayerMask l){
		RaycastHit2D hit;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		return (hit = Physics2D.GetRayIntersection(r,100f,l.value));
	}

	/*void revealArea(int i){
		List<NavigationNode> nodes = new List<NavigationNode>();
		nodes.Add(position);
		for(int j = 0; j < i; j++){
			List<NavigationNode> newNodes = new List<NavigationNode>();
			foreach(NavigationNode n in nodes){
				foreach(NavigationNode o in n.GetNeighbors()){
					newNodes.Add(o);
				}
			}
			foreach(NavigationNode b in newNodes){
				nodes.Add(b);
				b.setVisibility(true);
			}
		}
	}*/
}
