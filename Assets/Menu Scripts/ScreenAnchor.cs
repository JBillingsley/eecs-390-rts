using UnityEngine;
using System.Collections;

public class ScreenAnchor : MonoBehaviour {
	public GameObject ui;
	public float yOffSet;
	public float xOffSet;
	// Update is called once per frame
	void Update () {
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		
		Bounds bounds = ui.GetComponent<SpriteRenderer>().bounds;
		
		// Set a new vector to the top left of the scene 
		Vector3 topLeftPosition = new Vector3(-camHalfWidth, camHalfHeight, 0) + Camera.main.transform.position; 
		
		// Offset it by the size of the object 
		topLeftPosition += new Vector3(bounds.size.x - xOffSet,-bounds.size.y + yOffSet , 0);
		
		transform.position = topLeftPosition;        
	}
}
