using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour {

	public bool isInteractable = false;
	public Element elementType;
	public GameObject player;
	public bool resourceTaken = false;
	public float resourceRespawnTime;
	private float resourceTimer;

	public bool interactable(){
		return isInteractable;
	}

	public void setInteractable(){
		isInteractable = true;
	}

	public void setNotInteractable(){
		isInteractable = false;
	}
	
	public void takeResource(){
		setNotInteractable();
		resourceTaken = true;
		renderer.enabled = false;
		collider.enabled = false;
		resourceTimer = Time.time;
		player.GetComponent<PlayerController>().incResources(elementType);
	}

	// Update is called once per frame
	void Update () {

		if(isInteractable && Input.GetKeyDown(KeyCode.Mouse0)){
			takeResource();
		}

		if(resourceTaken && Time.timeSinceLevelLoad - resourceTimer > resourceRespawnTime){
			resourceTaken = false;
			renderer.enabled = true;
			collider.enabled = true;
		}
	}
}
