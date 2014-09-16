using UnityEngine;
using System.Collections;

public class CameraLevel : MonoBehaviour {

	public Transform initialCamPosition;

	// Update is called once per frame
	void Update () {
		if(transform.position.y > initialCamPosition.position.y || transform.position.y < initialCamPosition.position.y){
			transform.position.Set(transform.position.x, initialCamPosition.position.y, transform.position.z);
		}
	}
}
