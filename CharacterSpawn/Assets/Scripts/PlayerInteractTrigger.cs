using UnityEngine;
using System.Collections;

public class PlayerInteractTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		if(collider.tag == "Resource"){
			collider.GetComponent<ResourceManager>().setInteractable();
		}
	}

	void OnTriggerExit(Collider collider){
		if(collider.tag == "Resource"){
			collider.GetComponent<ResourceManager>().setNotInteractable();	
		}
	}
}
