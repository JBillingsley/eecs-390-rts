using UnityEngine;
using System.Collections;

public class ScreenAnchor : MonoBehaviour {
	public float yOffset;
	public float xOffset;
	// Update is called once per frame
	void Update () {
		Vector3 screenPosition = transform.position;
		screenPosition = Camera.main.WorldToViewportPoint(screenPosition);
		transform.position = Camera.main.ScreenToWorldPoint(screenPosition);
		screenPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
		transform.position = screenPosition;
	}
}
