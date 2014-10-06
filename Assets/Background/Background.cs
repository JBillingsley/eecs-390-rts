using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	public Camera camerax;
	public float scrollSpeed;
	private float lastX;

	void Update () {
		if (lastX != transform.position.x){
			float x = transform.position.x * scrollSpeed;
			GetComponent<MeshFilter>().mesh.uv = new Vector2[]{new Vector2(x, 1), new Vector2(1 + x, 0), new Vector2(x, 0), new Vector2(1 + x, 1)};
			lastX = transform.position.x;
		}
		float minHeight = Mathf.Min(Screen.width/2, Screen.height);
		float h = Mathf.Max(camerax.orthographicSize * 2);
		transform.localScale = new Vector2 (2*h, h);
		transform.localPosition = new Vector3 (-h, -h/2, transform.localPosition.z);
	}
}
