using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public ulong[,] map;

	public bool[,] dirtyChunks;

	public Tilesetx tileset;

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
		for (int y = 0; y < h*chunkSize; y++)
			for (int x = 0; x < w*chunkSize; x++){
				bool solid = Random.value > (float)y / (h*chunkSize);
				if (solid)
					map [y, x] = (ulong)(Random.value * 2) + 1;
			}
		for (int y = 0; y < h*chunkSize; y++)
			for (int x = 0; x < w*chunkSize; x++)
				updateTileData(new IVector2(x, y));
	}

	/********************/
	/*                  */
	/********************/

	public bool isSolid(IVector2 v){
		return getTileData (v).solid;
	}

	public TileData getTileData(IVector2 v){
		TileData t = TileData.tiles[getTileID(v)];
		if (t == null)
			return TileData.tiles[TileData.defaultTile];
		return t;
	}
	public byte getTileID(IVector2 v){
		if (!inBounds(v))
			return TileData.defaultTile;
		return getByte(v, 0);
	}
	public byte getTileSolid(IVector2 v){
		if (!inBounds(v))
			return 0;
		return getByte(v, 2);
	}
	public byte getTileNav(IVector2 v){
		if (!inBounds(v))
			return 0;
		return getByte(v, 3);
	}

	public byte getTileRender(IVector2 v){
		switch(renderMode){
			case NAVMESH:
				return getTileNav(v);
			default:
				return getTileSolid(v);
		}
	}
	public TileData getTileRenderData(IVector2 v){
		switch(renderMode){
			case NAVMESH:
				return (!isSolid(v) && isSolid(v + Direction.getDirection(Direction.BOTTOM)))? TileData.air : TileData.tiles[TileData.defaultTile];
			default:
				return getTileData(v);
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
		return (byte)(map [v.y, v.x] >> i * 8);
	}

	public void setAndUpdateTileData(IVector2 v, short id){
		setTileData (v, id);
		updateTileData(v);
	}

	public void setTileData(IVector2 v, short id){
		map [v.y,v.x] = (uint)id;
	}

	public void updateTileData(IVector2 v){
		map [v.y,v.x] &= 0x00FF;
		bool[] b = new bool[directions.Length];
	
		for (int i = 0; i < directions.Length; i++)
			b[i] = isSolid(v + Direction.getDirection(directions[i]));
		map [v.y,v.x] |= (uint)Direction.packByte(directions, b) << 16;

		for (int i = 0; i < directions.Length; i++)
			b[i] = isSolid(v + Direction.getDirection(directions[i])) || !isSolid(v + Direction.getDirection(directions[i]) + Direction.getDirection(Direction.BOTTOM));
		map [v.y,v.x] |= (uint)Direction.packByte(directions, b) << 24;
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

	public Tilesetx getTileset(){
		return tileset;
	}

	/********************/
	/*                  */
	/********************/
	[System.Serializable]
	public class Tilesetx{
		public Material material;
		public int xTiles = 1;
		public int yTiles = 1;
	}
}
