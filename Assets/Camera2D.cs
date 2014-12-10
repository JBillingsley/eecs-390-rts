﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera2D : MonoBehaviour {

	public Map map;

	private float zoom = 1;
	private float targetZoom = 1;
	public float minZoom = 0.25f;
	public float maxZoom = 2f;
	public float cameraSpeed = 0.5f;

	public int xend = 1;
	public int yend = 1;

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
		float w = (float)Screen.width / Screen.height * tileHeight() + xend * 2;
		float h = tileHeight() + yend * 2;
		float mw = map.getWidth();
		float mh = map.getHeight();
		x = Mathf.Clamp (x, w / 2, mw - w / 2);
		y = Mathf.Clamp (y, h / 2, mh - h / 2);
		transform.position = new Vector3(x, y, -10);
		debugRect (0, mw, 0, mh, Color.red);
		debugRect (w / 2, mw - w / 2, h / 2, mh - h / 2, Color.cyan);
		debugRect (0, w, 0, h, Color.green);
		debugRect (mw - w, mw, 0, h, Color.green);
		debugRect (mw - w, mw, mh - h, mh, Color.green);
		debugRect (0, w, mh - h, mh, Color.green);
	}

	private static void debugRect(float x1, float x2, float y1, float y2, Color c){
		Debug.DrawLine (new Vector3 (x1, y1, -10), new Vector3 (x1, y2, -10), c);
		Debug.DrawLine (new Vector3 (x2, y1, -10), new Vector3 (x2, y2, -10), c);
		Debug.DrawLine (new Vector3 (x1, y1, -10), new Vector3 (x2, y1, -10), c);
		Debug.DrawLine (new Vector3 (x1, y2, -10), new Vector3 (x2, y2, -10), c);
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
