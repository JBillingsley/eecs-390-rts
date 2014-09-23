using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public bool canSummon = false;
	public int avalibleResources = 0;
	public int maxUnitCount = 5;
	public bool healable = false;
	public GameObject tower;
	public int repairSpeed;

	// Use this for initialization
	void Start () {
	
	}

	public void incResources(){
		avalibleResources += 1;
	}

	public void decResources(){
		avalibleResources -= 1;
	}
	
	public void incMaxUnitCount(){
		maxUnitCount += 5;
	}
	
	public void canHeal(){
		healable = true;
	}
	
	public void cantHeal(){
		healable = false;
	}

	// Update is called once per frame
	void Update () {
		// This is the movement mechanic for the player
		transform.Translate(new Vector3(Input.GetAxis("Horizontal")*speed,0, 0));
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			rigidbody.AddForce(new Vector3(0f, jumpForce, 0f));
		}

		// This will summon a dude on Lmouse down if the conditions are met
		if(canSummon && Input.GetKeyDown(KeyCode.Mouse0) && avalibleResources > 0){
			transform.GetComponentInChildren<SpawnDude>().spawnDude();
			decResources();
		}
		
		if(healable && Input.GetKeyDown(KeyCode.Mouse1)){
			tower.GetComponent<TowerManager>().repair(repairSpeed);
		}
	}
}
