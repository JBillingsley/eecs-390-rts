using UnityEngine;
using System.Collections;

public class Unit : NPC {

	// Update is called once per frame
	void Update(){
		act ();
		move ();
	}

	protected override void attack(){
		if(!characterTarget){
			myState = State.IDLE;
			return;
		}
		Vector3 targetPos = characterTarget.position;
		if((lastTargetPosition-targetPos).magnitude > 1f){
			findPath(targetPos);
		}
		if((targetPos - this.transform.position).magnitude < 1f){
			damage(characterTarget);
		}
	}
	protected override void follow(){

	}
	protected override void gather(){
		// This element type should be determined by the element being mined
		Debug.Log ("gathering");
		InventroyManager.instance.addToInventory(Element.DIRT);
	}
	protected override void idle(){

	}

	public void interactWithPosition(Vector2 v){

	}

	public void gatherPosition(Vector2 v){
		myState = State.GATHERING;
	}

	public void attackPosition(Vector2 v){
		myState = State.ATTACKING;
		characterTarget = em.getNearestEnemy(v);
	}
}
