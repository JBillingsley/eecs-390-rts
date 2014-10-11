using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public ulong[,] map;

	public bool[,] dirtyChunks;

	public Tileset tileset;

	public int w = 1;
	public int h = 1;
	public int tileSize = 16;
	public int chunkSize = 16;

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
		for (int x = 0; x < w*chunkSize; x++)
			for (int y = 60 + (int)(Random.value * 3); y >= 0; y--){
				map [y, x] = (ulong)(Random.value * 2) + 1;
			}
		for (int y = 0; y < h*chunkSize; y++)
			for (int x = 0; x < w*chunkSize; x++)
				updateTileSpec(new IVector2(x, y));
	}

	/********************/
	/*                  */
	/********************/
	public const int FOREGROUND_ID = 0;
	public const int BACKGROUND_ID = 1;
	public const int FOREGROUND_CONTEXT = 2;
	public const int BACKGROUND_CONTEXT = 3;
	public const int NAVIGATION_MAP = 4;

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
			default:
				return getByte(v, FOREGROUND_CONTEXT);
		}
	}
	public TileSpec getRenderForeground(IVector2 v){
		switch(renderMode){
			case NAVMESH:
			return (!isForegroundSolid(v) && isForegroundSolid(v + Direction.getDirection(Direction.BOTTOM)))? TileSpecList.getTileSpec(0) : TileSpecList.getTileSpec(1);
			default:
				return getForeground(v);
		}
	}
	
	public TileSpec getRenderBackground(IVector2 v){
		switch(renderMode){
			case NAVMESH:
			return (!isForegroundSolid(v) && isForegroundSolid(v + Direction.getDirection(Direction.BOTTOM)))? TileSpecList.getTileSpec(2) : TileSpecList.getTileSpec(0);
			default:
				return getBackground(v);
		}
	}
	public const byte TERRAIN = 0;
	public const byte NAVMESH = 1;
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
		map [v.y,v.x] &= 0x00FF;
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
			tex [0] = new Vector3 (x/width, 1 - y1/height);
			tex [1] = new Vector3 (x/width, 1 - y/height);
			tex [2] = new Vector3 (x1/width, 1 - y/height);
			tex [3] = new Vector3 (x1/width, 1 - y1/height);
			return tex;
		}
	}
}
