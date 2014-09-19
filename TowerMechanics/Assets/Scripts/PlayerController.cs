using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Maximum number of units the player can spawn
	public int maxUnits;
	public int currentUnits;
	
	public int currentHealth;
	public int maxHealth;
	
	/**
	 * This method is called from enemy units to the player when the player is being attacked
	 */
	public void hurt(int damage){
		currentHealth -= damage;
	}
	
	/**
	 * This is called by the tower when a tower is upgraded
	 * This will increase the maximum number of units the player is allowed to spawn
	 */
	public void incMaxUnitCount(){
		// this is a temporary increase value
		maxUnits += 5;
	}
	
	/**
	 * this spawns units for the player when the player is on the tower
	 * Units can only be spawned up to the maximum number of units given as madUnits
	 */
	 public void spawnUnit(){
	 	// We will probably need some input params
	 	if(currentUnits < maxUnits){
	 		currentUnits += 1;
	 		// Spawn a unit
	 	}
	 	// else we are at maximum unit capacity and we cant spawn units 
	 }
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
