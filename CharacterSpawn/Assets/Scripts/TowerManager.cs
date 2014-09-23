using UnityEngine;
using System.Collections;

public class TowerManager : MonoBehaviour {
	
	public int currentHealth;
	public int maxHealth;
	public GameObject[] towerLayers;
	public Element towerType;
	
	/**
	 * TODO:
	 * Send a message to player when tower is taking damage
	 * add respawn point
	 * make prgression system for tower (that might be part of crafting)
	 */
	
	/**
	 * removes an input amount of hitpoints from the tower
	 */
	public void takeDamage(int damage){
		// TODO First we should try and send a message to the player (we can do this on a timer or something)
		this.currentHealth -= damage;
	}
	
	/**
	 * regenerates the tower health by the input amount
	 */
	public void repair(int hitpoints){
		/* TODO We also need to raise the effected tower layer by a constant amount
		 * also tell all units to stop repairing
		 */
		if(currentHealth < maxHealth){
			currentHealth += hitpoints;
		}
		
		// After the tower finishes being repaired we should make sure the current health never exceeds maxHealth
		if(currentHealth > maxHealth){
			currentHealth = maxHealth;
		}
		// Somewhere here we should tell the units to stop working
	}
	
	/**
	 * Initiates the building process for the next layer of the tower
	 * This will probably be called by some other method
	 */
	public void initiateTowerUpgrade(){
		towerType += 1;
		// Calls the private upgrade tower method
		this.upgradeTower ();
	}
	
	/**
	 * runs through the tower upgrade process
	 */
	private void upgradeTower(){
		// Add a new tower layer
		// update textures for all towers
		// increase max health points TODO(this is just a random number for now)
		this.maxHealth += 50;
		
		// gets the next tower layer and enables its colliders and renderers
		GameObject nextTowerLayer = towerLayers[(int)towerType];
		nextTowerLayer.renderer.enabled = true;
		nextTowerLayer.collider.enabled = true;
		// Should also probably change the textures when those are a thing
		
		// This is a fuction call to the player to increase the max units spawnable
		// I think there is a better way to do this than gameObject.find
		GameObject.Find("Player").GetComponent<PlayerController>().incMaxUnitCount();
	}
	
	/**
	 * updates the total ammount of units the player can spawn
	 * TODO: determine if this function will be linear or use some other kind of scaling mechanism
	 */
	public int increaseUnitCap(){
		// Calls an update method to the player controller with a number
		return -1;
	}
	
	// Use this for initialization
	void Start () {
		// When the tower is initialized it will be of type DIRT
		towerType = Element.DIRT;
		
		// Runs through all the tower layers and deactivates their renderers and colliders
		foreach(GameObject towerLayer in towerLayers){
			towerLayer.renderer.enabled = false;
			towerLayer.collider.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
