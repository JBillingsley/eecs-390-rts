using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jumpForce;
	public bool canSummon = false;
	public int avalibleResources = 0;
	public int maxUnitCount = 5;

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

	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(Input.GetAxis("Horizontal")*speed,0, 0));
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			rigidbody.AddForce(new Vector3(0f, jumpForce, 0f));
		}

		if(canSummon && Input.GetKeyDown(KeyCode.Mouse0) && avalibleResources > 0){
			transform.GetComponentInChildren<SpawnDude>().spawnDude();
			decResources();
		}
	}
}
