using UnityEngine;
using System.Collections;

public class SummoningTrigger : MonoBehaviour {


	void OnTriggerEnter(Collider collider){
		if(collider.tag == "Player")
			collider.GetComponent<PlayerController>().canSummon = true;
	}

	void OnTriggerExit(Collider collider){
		if(collider.tag == "Player")
			collider.GetComponent<PlayerController>().canSummon = false;
	}
}
