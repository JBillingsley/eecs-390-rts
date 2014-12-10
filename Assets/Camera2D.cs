using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera2D : MonoBehaviour {

	public Map map;

	private float zoom = 1;
	private float targetZoom = 1;
	public float minZoom = 0.25f;
	public float maxZoom = 2f;
	public float cameraSpeed = 0.5f;

	private static Dictionary<int, Chunk> chunks = new Dictionary<int, Chunk>();

	public Transform lockonTarget;
	public bool lockedOn;
	
	void Update () {
		zoom = zoom + (targetZoom - zoom) * 0.1f;
		camera.orthographicSize = tileHeight()/2f;
		focus();
	//	chunkManagement();
	}

	public void move(Vector2 delta){
		camera.transform.position += (Vector3)(delta * cameraSpeed * zoom);
	}

	public void zoomIn(){
		float newZoom = targetZoom;
		newZoom *= 2f;
		targetZoom = Mathf.Min(maxZoom, Mathf.Max(minZoom, newZoom));
	}
	public void zoomOut(){
		float newZoom = targetZoom;
		newZoom /= 2f;
		targetZoom = Mathf.Min(maxZoom, Mathf.Max(minZoom, newZoom));	
	}

	private void chunkManagement(){
		int xChunks = chunkWidth() / 2 + 1;
		int yChunks = chunkHeight() / 2 + 1;
		short x0 = (short)(transform.position.x / map.chunkSize);
		short y0 = (short)(transform.position.y / map.chunkSize);
		for (short x = (short)Mathf.Max(x0 - xChunks, 0); x <= (short)Mathf.Min(x0 + xChunks, map.w - 1); x++)
			for (short y = (short)Mathf.Max(y0 - yChunks, 0); y <= (short)Mathf.Min(y0 + yChunks, map.w - 1); y++)
				showChunk(map, x, y);
		List<int> keys = new List<int>();
		foreach(KeyValuePair<int, Chunk> entry in chunks){
			short xx = (short)(entry.Key >> 16);
			short yy = (short)(entry.Key & 0xFFFF);
			if (xx < x0 - xChunks || xx > x0 + xChunks || yy < y0 - yChunks || yy > y0 + yChunks)
				keys.Add(entry.Key);
		}
		foreach(int key in keys){
			chunks[key].destroy();
			chunks.Remove(key);
		}
	}

	public void focus (){
		float x = lockonTarget.position.x + .5f;
		float y = lockonTarget.position.y +.5f;
		float w = Screen.width / Screen.height * tileHeight();
		float h = tileHeight();
		float mw = map.getWidth();
		float mh = map.getHeight();
		x = Mathf.Clamp (x, w / 2, mw - w / 2);
		y = Mathf.Clamp (y, h / 2, mh - h / 2);
		transform.position = new Vector3(x, y);
	}

	private static void showChunk(Map map, short x, short y){
		int key = (x << 16) + y;
		Debug.Log (key);
		if (!chunks.ContainsKey(key)){
			Debug.Log ("Building Chunk " + key + ", " + x + "," +y);
			chunks [key] = Chunk.makeChunk (map, x, y);
		}
	}

	private float tileHeight(){
		return (float)(Screen.height) * zoom / map.tileSize;
	}

	private int chunkWidth(){
		return (int)(Screen.width * zoom / (map.tileSize * map.chunkSize));
	}	

	private int chunkHeight(){
		return (int)(Screen.height * zoom / (map.tileSize * map.chunkSize));
	}

	private void lockOn(Transform t){
		lockonTarget = t;
		lockedOn = true;
	}

	private void unLock(){
		lockedOn = false;
	}
}
