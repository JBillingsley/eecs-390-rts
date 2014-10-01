using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public bool canSummon = false;
	// This is janky right now but this is the length because its the last element + 1 
	public int[] avalibleResources;
	public int maxUnitCount = 5;
	public bool healable = false;
	public GameObject tower;
	public int repairSpeed;
	public Element dudeSpawnType;

	// Use this for initialization
	void Start () {
		avalibleResources = new int[(int)Element.ROCK + 1];
	}

	public void incResources(Element type){
		avalibleResources[(int)type] += 1;
	}
	
	public int[] getAllResources(){
		return avalibleResources;
	}

	public void decResources(Element type){
		avalibleResources[(int)type] -= 1;
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
		int availResourcesForDude = avalibleResources[(int)dudeSpawnType];
		if(canSummon && Input.GetKeyDown(KeyCode.Mouse0) && availResourcesForDude > 0){
			transform.GetComponentInChildren<SpawnDude>().spawnDude();
			decResources(dudeSpawnType);
		}
		
		if(healable && Input.GetKeyDown(KeyCode.Mouse1)){
			tower.GetComponent<TowerManager>().repair(repairSpeed);
		}
		
		if(healable && Input.GetKeyDown(KeyCode.U)){
			tower.GetComponent<TowerManager>().initiateTowerUpgrade();
		}
	}
}
