using UnityEngine;
using System.Collections;

public abstract class NPC : Character {

	protected enum State {IDLE,TRAVELING,GATHERING,REPAIRING,ATTACKING,FOLLOWING};
	public enum Team {USER,HUMAN,FISH,MOLE,BIRD,EVIL};

	protected State myState;
	protected Team myTeam;
}
