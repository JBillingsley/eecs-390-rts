using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

	private MeshFilter meshFilter;
	private GameObject renderData;
	private GameObject colliderData;

	void Start () {
		transform.parent = map.transform;
		name = "Chunk" + Util.pair (x, y);


		renderData = wrapMesh("Render Data" + Util.pair (x, y), makeRenderMesh (x, y));
		renderData.AddComponent("MeshRenderer");
		renderData.renderer.material = map.getTileset().material;

		colliderData = wrapMesh("Collider Data" + Util.pair (x, y), makeCollisionMesh (x, y));
		colliderData.AddComponent("MeshCollider");
		//colliderData.AddComponent("MeshRenderer");

	}

	/*********************************************************************************************/
	/*********************************************************************************************/
	/*********************************************************************************************/
	
	private Map map;
	private short x, y;

	void Update () {
		refresh();
	}

	public void refresh(){
		if (map.isDirty (x, y)){
			MeshFilter rFilter = renderData.GetComponent<MeshFilter>();
			rFilter.sharedMesh.uv = makeTextures(x, y);
			rFilter.sharedMesh.RecalculateBounds();
			MeshFilter cFilter = colliderData.GetComponent<MeshFilter>();
			MeshCollider cCollider = colliderData.GetComponent<MeshCollider>();
			cFilter.sharedMesh.triangles = makeCollisionIndices(x, y);
			cFilter.sharedMesh.RecalculateBounds();
			cCollider.sharedMesh = null;
			cCollider.sharedMesh = cFilter.sharedMesh;
			map.unDirty(x, y);
		}
	}

	public void destroy(){
		/*map.makeDirty (x, y);
		Destroy(renderData.GetComponent<MeshFilter>().sharedMesh);
		Destroy(colliderData.GetComponent<MeshFilter>().sharedMesh);
		Destroy(gameObject);*/
	}

	/*********************************************************************************************/
	/*********************************************************************************************/
	/*********************************************************************************************/
	
	public static Chunk makeChunk(Map map, short x, short y){
		GameObject obj = new GameObject();
		obj.transform.position = new Vector2 (x * map.chunkSize, y * map.chunkSize);
		Chunk chunk = (Chunk) obj.AddComponent("Chunk");
		chunk.map = map;
		chunk.x = x;
		chunk.y = y;
		obj.layer = 8;
		map.unDirty (x, y);
		return chunk;
	}
	
	private GameObject wrapMesh(string name, Mesh mesh){
		GameObject obj = new GameObject(name);
		((MeshFilter)obj.AddComponent("MeshFilter")).sharedMesh = mesh;
		obj.transform.parent = transform;
		obj.transform.position = transform.position;
		obj.layer = gameObject.layer;
		return obj;
	}

	/*********************************************************************************************/
	/*********************************************************************************************/
	/*********************************************************************************************/

	public static void init(Map map){
		chunkVertices = makeVertices(map.chunkSize);
		chunkIndices = makeIndices(map.chunkSize);
	}

	private static Vector3[] chunkVertices;
	private static int[] chunkIndices;

	/*********************************************************************************************/
	/*********************************************************************************************/
	/*********************************************************************************************/

	private Mesh makeRenderMesh(int x, int y){
		Mesh mesh = new Mesh ();
		mesh.name = "Chunk Render Mesh" + Util.pair(x, y);
		mesh.vertices = chunkVertices;
		mesh.triangles = chunkIndices;
		mesh.uv = makeTextures(x, y);
		return mesh;
	}

	private Mesh makeCollisionMesh(int x, int y){
		Mesh mesh = new Mesh ();
		mesh.name = "Chunk Collision Mesh" + Util.pair(x, y);
		mesh.vertices = chunkVertices;
		mesh.triangles = makeCollisionIndices(x, y);
		return mesh;
	}

	/*********************************************************************************************/
	/*********************************************************************************************/
	/***********************************************************************************************/

	private static Vector3[] makeVertices(int chunkSize){
		Vector3[] vertices = new Vector3[8 * chunkSize * chunkSize];
		int i = 0;
		for (int y = 0; y < chunkSize; y++) {
			for (int x = 0; x < chunkSize; x++) {
				vertices [i*4] = new Vector3 (x, y, -1);
				vertices [i*4+1] = new Vector3 (x, y+1, -1);
				vertices [i*4+2] = new Vector3 (x+1, y+1, -1);
				vertices [i*4+3] = new Vector3 (x+1, y, -1);
				i++;
			}
		}
		for (int y = 0; y < chunkSize; y++) {
			for (int x = 0; x < chunkSize; x++) {
				vertices [i*4] = new Vector3 (x, y, 1);
				vertices [i*4+1] = new Vector3 (x, y+1, 1);
				vertices [i*4+2] = new Vector3 (x+1, y+1, 1);
				vertices [i*4+3] = new Vector3 (x+1, y, 1);
				i++;
			}
		}
		return vertices;
	}
	
	private static int[] makeIndices(int chunkSize){
		int[] indices = new int[12 * chunkSize * chunkSize];
		for (int i = 0; i < 2 * chunkSize * chunkSize; i++) {
			indices [i*6] = i*4;
			indices [i*6+1] = i*4 + 1;
			indices [i*6+2] = i*4 + 3;
			indices [i*6+3] = i*4 + 1;
			indices [i*6+4] = i*4 + 2;
			indices [i*6+5] = i*4 + 3;
		}
		return indices;
	}

	private Vector2[] makeTextures(int cx, int cy){
		Vector2[] textures = new Vector2[8 * map.chunkSize * map.chunkSize];
		float ix = 1f / map.tileset.width;
		float iy = 1f / map.tileset.height;
		int i = 0;
		for (int y = 0; y < map.chunkSize; y++) {
			for (int x = 0; x < map.chunkSize; x++) {
				IVector2 v = new IVector2(cx*map.chunkSize + x, cy*map.chunkSize + y);
				TileSpec tile = map.getRenderForeground(v);
				int texID = tile.getTexID(map.getRenderContext(v));
				Vector2[] tex = map.tileset.getTex(texID);
				textures [i*4] = tex[0];
				textures [i*4+1] = tex[1];
				textures [i*4+2] = tex[2];
				textures [i*4+3] = tex[3];
				i++;
			}
		}
		for (int y = 0; y < map.chunkSize; y++) {
			for (int x = 0; x < map.chunkSize; x++) {
				IVector2 v = new IVector2(cx*map.chunkSize + x, cy*map.chunkSize + y);
				TileSpec tile = map.getRenderBackground(v);
				int texID = tile.getTexID(map.getBackgroundRenderContext(v));
				Vector2[] tex = map.tileset.getTex(texID);
				textures [i*4] = tex[0];
				textures [i*4+1] = tex[1];
				textures [i*4+2] = tex[2];
				textures [i*4+3] = tex[3];
				i++;
			}
		}
		return textures;
	}

	private int[] makeCollisionIndices(int cx, int cy){
		List<int> indices = new List<int>();
		int i = 0;
		int off = 4 * map.chunkSize * map.chunkSize;
		IVector2 v;
		for (int y = 0; y < map.chunkSize; y++) {
			for (int x = 0; x < map.chunkSize; x++, i++) {
				v = new IVector2(cx*map.chunkSize + x, cy*map.chunkSize + y);
				TileSpec tile = map.getForeground(v);
				if (tile.solid)
					tile.getCollisionIndices(4*i, off, (byte)~map.getRenderContext(v), indices);
			}
		}
		return indices.ToArray();
	}

}
