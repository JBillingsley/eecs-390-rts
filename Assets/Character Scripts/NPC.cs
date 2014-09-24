using UnityEngine;
using System.Collections;

public abstract class NPC : Character {

	public enum State {ATTACKING,FOLLOWING,GATHERING,IDLE};
	public enum Team {FRIENDLY,HUMAN,FISH,MOLE,BIRD,HOSTILE};

	public State myState;
	public Team myTeam;

	public Character characterTarget;
	public Vector2 lastTargetPosition;

	//Perform an action based on the current state
	public void act(){
		switch(myState){
		case State.ATTACKING:
			attack();
			break;
		case State.FOLLOWING:
			follow();
			break;
		case State.GATHERING:
			gather();
			break;
		case State.IDLE:
			idle();
			break;
		}
	}

	protected abstract void attack();
	protected abstract void follow();
	protected abstract void gather();
	protected abstract void idle();

}
