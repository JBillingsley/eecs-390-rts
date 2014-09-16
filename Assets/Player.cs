using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Character {
	
	public static Player singleton;

	public LayerMask terrainLayer;
	public LayerMask itemLayer;

	//public Item equipped;

	//private NavigationNode lastPosition;

	// Use this for initialization
	void Start () {
		singleton = this;
		//position = null;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(position == null){
			position = Spawner.singleton.l[0,0];
			transform.position = position.transform.position;
			lastPosition = position;
			position.setVisibility(true);
			position.setOpen(true);
			revealArea(3);
		}
		if(Input.GetMouseButtonDown(0)){
			RaycastHit2D itemHit;
			RaycastHit2D terrainHit;
			if(itemHit = getCollider(itemLayer)){
				pickupItem(itemHit);
			}
			else if(terrainHit = getCollider(terrainLayer)){
				if(equipped){
					equipped.use(terrainHit.point);
					equipped = null;
				}
			}

		}
		else if(Input.GetMouseButtonDown(1)){
			RaycastHit2D hit = new RaycastHit2D();
			if(hit = getCollider(terrainLayer)){
				Collider2D col = hit.collider;
				NavigationNode node = col.GetComponent<NavigationNode>();
				Character c = this;
				if(node.open){
					position.goal = node;
					Invoke("makeRoute",0f);
				}
			}
		}
		if(lastPosition != position){
			lastPosition = position;
			revealArea(3);
		}
		move();*/
	}

	/*void makeRoute(){
		Route r = this.position.getRoute(position.goal);
		this.setPath(r);
	}*/

	/*void pickupItem(RaycastHit2D hit){
		Debug.Log ("equipped");
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
