using UnityEngine;
using System.Collections;

public class PlayerInteractTrigger : MonoBehaviour {

	/* For the interact range, these are objects the player can interact with
		We will set different tags as interactable when the player is in this range */
	void OnTriggerEnter(Collider collider){
	// the player will interact with resources
		if(collider.tag == "Resource"){
			collider.GetComponent<ResourceManager>().setInteractable();
		}
		
		if(collider.tag == "Tower"){
			transform.parent.GetComponent<PlayerController>().canHeal();
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.tag == "Resource"){
			collider.GetComponent<ResourceManager>().setNotInteractable();	
		}
		
		if(collider.tag == "Tower"){
			transform.parent.GetComponent<PlayerController>().cantHeal();
		}
	}
}
