using UnityEngine;
using System.Collections;

public class Enemy : NPC {

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
		if(!characterTarget){
			myState = State.IDLE;
			return;
		}
		Vector3 targetPos = characterTarget.position;
		if((lastTargetPosition-targetPos).magnitude > 2f){
			findPath(targetPos);
		}
	}

	protected override void gather(){

	}

	protected override void idle(){
		if(characterTarget){
			myState = State.FOLLOWING;
		}
	}

}
