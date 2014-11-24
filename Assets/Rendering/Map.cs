using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public ulong[,] map;

	public bool[,] dirtyChunks;

	public Tileset tileset;

	public int w = 1;
	public int h = 1;
	public int tileSize = 16;
	public int chunkSize = 16;

	public int surfaceHeight = 60;
	public int surfaceRandomness = 2;

	public List<TileCluster> rects = new List<TileCluster>();

	private int[] directions = new int[] {
		Direction.TOPLEFT,
		Direction.TOP,
		Direction.TOPRIGHT,
		Direction.RIGHT,
		Direction.BOTTOMRIGHT,
		Direction.BOTTOM,
		Direction.BOTTOMLEFT,
		Direction.LEFT
	};

	
	public void Awake(){

		Chunk.init (this);
		name = name + Util.pair(getWidth(), getHeight());
		map = new ulong[h*chunkSize, w*chunkSize];
		dirtyChunks = new bool[h, w];
		for (int x = 0; x < w*chunkSize; x++){
			for (int y = surfaceHeight + (int)(Random.value * surfaceRandomness); y >= 0; y--)
				setByte(new IVector2(x, y), FOREGROUND_ID, randomTile(x,y)); //Change this line!!!!!!
			for (int y = surfaceHeight + 1 + (int)(Random.value * surfaceRandomness); y >= 0; y--)
 				setByte(new IVector2(x, y), BACKGROUND_ID, (byte)1);
		}

		foreach(TileCluster tc in rects){
			populateCluster(tc);
		}

		for (int y = 0; y < h*chunkSize; y++)
			for (int x = 0; x < w*chunkSize; x++)
				updateTileSpec(new IVector2(x, y));
	}

	public byte randomTile(int x, int y){
		while(true){
			int i = (int)(Random.value * 32);
			byte b = (byte)(Random.value * 9 + 2);
			if (TileSpecList.getTileSpec(b).weight >= i){
				if((int)b <= 3){
					b = (byte)(y < surfaceHeight?3:2);
				}
				return b;
			}
		}
	}

	public void populateCluster(TileCluster t){
		for(int x = t.x; x < t.x + t.width; x++){
			for(int y = t.y; y < t.y + t.height; y++){
				if(Random.value < t.likeliness){
					byte layerID = (byte)(t.background?(BACKGROUND_ID):FOREGROUND_ID);
					setByte(new IVector2(x,y),layerID,(byte)t.tileType);
				}
			}
		}
	}

	/********************/
	/*                  */
	/********************/
	public const int FOREGROUND_ID = 0;
	public const int BACKGROUND_ID = 1;
	public const int FOREGROUND_CONTEXT = 2;
	public const int BACKGROUND_CONTEXT = 3;
	public const int DURABILITY = 4;
	public const int NAVIGATION_MAP = 5;

	public bool isForegroundSolid(IVector2 v){
		return getForeground(v).solid;
	}
	public bool isBackgroundSolid(IVector2 v){
		return getBackground(v).solid;
	}

	public TileSpec getForeground(IVector2 v){
		TileSpec t = TileSpecList.getTileSpec(getByte(v, FOREGROUND_ID));
		if (t == null)
			return TileSpecList.getTileSpec(0);
		return t;
	}
	public TileSpec getBackground(IVector2 v){
		TileSpec t = TileSpecList.getTileSpec(getByte(v, BACKGROUND_ID));
		if (t == null)
			return TileSpecList.getTileSpec(0);
		return t;
	}

	public byte getRenderContext(IVector2 v){
		switch(renderMode){
			case NAVMESH:
				return getByte(v, NAVIGATION_MAP);
			case BACKGROUND:
				return getByte(v, BACKGROUND_CONTEXT);
			default:
				return getByte(v, FOREGROUND_CONTEXT);
		}
	}

	public byte getBackgroundRenderContext(IVector2 v){
		switch(renderMode){
			case NAVMESH:
				return 0;
			default:
				return getByte(v, BACKGROUND_CONTEXT);
		}
	}
	public TileSpec getRenderForeground(IVector2 v){
		switch(renderMode){
			case NAVMESH:
				return (!isForegroundSolid(v) && isForegroundSolid(v + Direction.getDirection(Direction.BOTTOM)))? TileSpecList.getTileSpec(0) : TileSpecList.getTileSpec(1);
			case BACKGROUND:
				return TileSpecList.getTileSpec(0);
			default:
				return getForeground(v);
		}
	}
	
	public TileSpec getRenderBackground(IVector2 v){
		switch(renderMode){
			case NAVMESH:
				return TileSpecList.getTileSpec(0);
			case BACKGROUND:
				return TileSpecList.getTileSpec(0);
			default:
				return getBackground(v);
		}
	}

	public const byte TERRAIN = 0;
	public const byte NAVMESH = 1;
	public const byte BACKGROUND = 2;
	public byte renderMode = NAVMESH;
	public void setRenderMode(byte mode){
		if (renderMode == mode)
			return;
		renderMode = mode;
		for (int y = 0; y < dirtyChunks.GetLength(0); y++)
			for (int x = 0; x < dirtyChunks.GetLength(1); x++)
				dirtyChunks[y,x] = true;
	}

	private bool inBounds(IVector2 v){
		return !((v.x < 0 || v.x >= getWidth()) || (v.y < 0 || v.y >= getHeight()));
	}
	/************************************/
	/************************************/

	public static IVector2 getMouseCoords(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float sfac = ray.origin.z/ray.direction.z;
		Vector2 pos = ray.origin + sfac * ray.direction;
		return new IVector2 ((int)pos.x, (int)pos.y);
	}
	/************************************/
	/*	X = tileID						*/
	/*	O = NavData						*/
	/*	T = SolidData					*/
	/*	OOOOOOOOTTTTTTTTXXXXXXXXXXXXXXXX*/
	/************************************/

	public byte getByte(IVector2 v, byte i){
		if (!inBounds(v))
			return 0;
		return (byte)(map [v.y, v.x] >> i * 8);
	}

	public byte getByte(IVector2 v, byte i, byte def){
		if (!inBounds(v))
			return def;
		return (byte)(map [v.y, v.x] >> i * 8);
	}

	public void setByte(IVector2 v, byte i, byte data){
		if (!inBounds(v))
			return;
		map [v.y, v.x] = (map [v.y, v.x] & ~((ulong)0xFF << (i * 8))) | ((ulong)data << (i * 8));
	}

	public void updateTileSpec(IVector2 v){
		if (!inBounds(v))
			return;

		map [v.y,v.x] &= 0xFFFF;
		bool[] b = new bool[directions.Length];
	
		for (int i = 0; i < directions.Length; i++)
			b[i] = isForegroundSolid(v + Direction.getDirection(directions[i]));
		setByte(v, FOREGROUND_CONTEXT, Direction.packByte(directions, b));

		for (int i = 0; i < directions.Length; i++)
			b[i] = isBackgroundSolid(v + Direction.getDirection(directions[i]));
		setByte(v, BACKGROUND_CONTEXT, Direction.packByte(directions, b));

		for (int i = 0; i < directions.Length; i++)
			b[i] = isForegroundSolid(v + Direction.getDirection(directions[i])) || !isForegroundSolid(v + Direction.getDirection(directions[i]) + Direction.getDirection(Direction.BOTTOM));
		setByte(v, NAVIGATION_MAP, Direction.packByte(directions, b));
	}

	public void setTile(IVector2 v, byte foreground, byte background){
		if (!inBounds(v))
			return;
		setByte (v, FOREGROUND_ID, foreground);
		setByte (v, BACKGROUND_ID, background);
		TileSpec t = TileSpecList.getTileSpec(foreground);
		byte d = 1;
		if (t != null)
			d = t.durability;
		setByte (v, DURABILITY, d);
		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++){
				IVector2 vi = v + new IVector2(x, y);
				updateTileSpec(vi);
				if (inBounds(vi))
					makeDirty(vi.x / chunkSize, vi.y / chunkSize);		
			}
	}


	/********************/
	/*                  */
	/********************/

	public int getWidth(){
		return w * chunkSize;
	}

	public int getHeight(){
		return h * chunkSize;
	}

	/********************/
	/*                  */
	/********************/

	public void makeDirty(int x, int y){
		dirtyChunks [y, x] = true;
	}

	public bool isDirty(int x, int y){
		return dirtyChunks [y, x];
	}

	public void unDirty(int x, int y){
		dirtyChunks [y, x] = false;
	}

	/********************/
	/*                  */
	/********************/

	public Tileset getTileset(){
		return tileset;
	}

	/********************/
	/*                  */
	/********************/
	[System.Serializable]
	public class Tileset{
		public Material material;
		public int width = 1;
		public int height = 1;

		public Vector2[] getTex(int i){
			Vector2[] tex = new Vector2[4];
			float x = i % width;
			float y = i / width;
			float x1 = x + 1;
			float y1 = y + 1;
			float dx = 1f/material.mainTexture.width;
			float dy = 1f/material.mainTexture.height;
			tex [0] = new Vector3 (x/width + dx, 1 - y1/height + dy);
			tex [1] = new Vector3 (x/width + dx, 1 - y/height - dy);
			tex [2] = new Vector3 (x1/width - dx, 1 - y/height - dy);
			tex [3] = new Vector3 (x1/width - dx, 1 - y1/height + dy);
			return tex;
		}
	}
}
