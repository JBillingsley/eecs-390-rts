using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {
	
	public float xOffset;
	public float speed;
	float camHalfHeight;
	Vector3 initPosition;
	
	// Use this for initialization
	void Start () {
		camHalfHeight = Camera.main.orthographicSize;
		initPosition = new Vector3(xOffset, -(Camera.main.transform.position.y + renderer.bounds.size.y), 0f);
		transform.position = initPosition;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f, speed * Time.deltaTime, 0f);
		if(transform.position.y > renderer.bounds.size.y){
			transform.position = initPosition;
		}	
	}
}
