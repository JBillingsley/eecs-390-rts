using UnityEngine;
using System.Collections;

public class ControlInput : MonoBehaviour {

	public Camera2D camera;

	void FixedUpdate () {
		Vector3 delta = new Vector2();
		if (Input.GetKey(KeyCode.UpArrow))
			delta.y += 1;
		if (Input.GetKey(KeyCode.DownArrow))
			delta.y -= 1;
		if (Input.GetKey(KeyCode.LeftArrow))
			delta.x -= 1;
		if (Input.GetKey(KeyCode.RightArrow))
			delta.x += 1;
		camera.transform.position += delta;
		
		float f = Input.GetAxis("Mouse ScrollWheel");
		if (f != 0){
			if (f > 0)
				camera.zoomIn();
			else
				camera.zoomOut();
		}
	}
}
