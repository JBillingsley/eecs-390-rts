using UnityEngine;
using System.Collections;

public class Unit : NPC {

	// Update is called once per frame
	void Update(){
		act ();
		move ();
	}

	protected override void attack(){

	}
	protected override void follow(){

	}
	protected override void gather(){

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
