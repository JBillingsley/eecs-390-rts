using UnityEngine;
using System.Collections;

public class SpawnDude : MonoBehaviour {

	public GameObject dude;
	public GameObject parent;

	public void spawnDude(){
		GameObject newDude = (GameObject)Instantiate(dude, transform.position, Quaternion.identity);
		newDude.transform.parent = parent.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
