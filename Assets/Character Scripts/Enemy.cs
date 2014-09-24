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
		}
		Vector2 targetPos = characterTarget.position;
		if((lastTargetPosition-targetPos).magnitude > 4f){
			lastTargetPosition = targetPos;
			setPath(getPath(position,targetPos));
		}
		if((targetPos - position).magnitude < 1f){
			hit(characterTarget);
		}
	}

	protected override void follow(){
		if(!characterTarget){
			myState = State.IDLE;
		}
		Vector2 targetPos = characterTarget.position;
		if((lastTargetPosition-targetPos).magnitude > 2f){
			lastTargetPosition = targetPos;
			setPath(getPath(position,new Vector2((int)targetPos.x,(int)targetPos.y)));
		}
	}

	protected override void gather(){

	}

	protected override void idle(){

	}

	void hit(Character c){
		c.currentHealth--;
	}
}
