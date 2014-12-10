using UnityEngine;
using System.Collections;

public class Enemy : NPC {

	public int spawnedTile;
	private float mineTime = 3;

	void Update(){
		act ();
		move ();
		if(!moving && !pathing){
			eliminate();
		}
	}

	void eliminate(){
		Destroy(this.gameObject);
	}

	protected override void mine(IVector2 v){
		byte d = map.getByte (v, Map.DURABILITY);
		if (mineTime <= 0){
			digging = false;
			TileSpec ts = TileSpecList.getTileSpec(map.getByte(v,Map.FOREGROUND_ID));
			map.setTile(v,(byte)spawnedTile,map.getByte(v,Map.BACKGROUND_ID));
			eliminate();
		}
		else{
			mineTime -= Time.fixedDeltaTime;
		}
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
