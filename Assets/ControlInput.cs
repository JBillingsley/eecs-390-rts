using UnityEngine;
using System.Collections;

public class ControlInput : MonoBehaviour {

	public Camera2D camerax;

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
		camerax.move (delta);

		if (Input.GetKey(KeyCode.KeypadMinus))
			camerax.zoomIn();
		if (Input.GetKey(KeyCode.KeypadPlus))
			camerax.zoomOut();

		float f = Input.GetAxis("Mouse ScrollWheel");
		if (f != 0){
			if (f > 0)
				camerax.zoomIn();
			else
				camerax.zoomOut();
		}
	}
}
