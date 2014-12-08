using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundScript : MonoBehaviour {
	
	public float xOffset;
	public float speed;
	float camHalfHeight;
	Vector3 initPosition;
	float randomSpeedFactor;

	
	// Use this for initialization
	void Start () {
		randomSpeedFactor = Random.Range(0.5f, 1.5f);
		camHalfHeight = Camera.main.orthographicSize;
		initPosition = new Vector3(xOffset, -(Camera.main.transform.position.y + renderer.bounds.size.y), 0f);
		transform.position = initPosition;
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f, speed * randomSpeedFactor * Time.deltaTime, 0f);
		if(transform.position.y > renderer.bounds.size.y){
			transform.position = initPosition;
			randomSpeedFactor = Random.Range(.5f, 1.5f);
		}	
	}
}
