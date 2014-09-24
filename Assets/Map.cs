using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour{

	public uint[,] map;

	public bool[,] dirtyChunks;

	public Tileset tileset;

	public int w = 1;
	public int h = 1;
	public int tileSize = 16;
	public int chunkSize = 16;

	public void Awake(){
		Chunk.init (this);
		name = name + Util.pair(getWidth(), getHeight());
		map = new uint[h*chunkSize, w*chunkSize];
		dirtyChunks = new bool[h, w];
		for (int y = 0; y < h*chunkSize; y++)
			for (int x = 0; x < w*chunkSize; x++){
				bool solid = Random.value > (float)y / (h*chunkSize);
				if (solid)
					map [y, x] = (uint)(Random.value * 2) + 1;
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
		TileData t = TileData.tiles[getTileID(v.x, v.y)];
		if (t == null)
			return TileData.tiles[TileData.defaultTile];
		return t;
	}
	public TileData getTileData(int x, int y){
		TileData t = TileData.tiles[getTileID(x, y)];
		if (t == null)
			return TileData.tiles[TileData.defaultTile];
		return t;
	}
	public short getTileID(int x, int y){
		if (x < 0 || x >= getWidth())
			return TileData.defaultTile;
		if (y < 0 || y >= getHeight())
			return TileData.defaultTile;
		return (short)map[y, x];
	}
	public byte getTileAdjacency(int x, int y){
		if (x < 0 || x >= getWidth())
			return 0;
		if (y < 0 || y >= getHeight())
			return 0;
		return (byte)(map[y, x]>>16);
	}
	public byte getTilePathing(int x, int y){
		if (x < 0 || x >= getWidth())
			return 0;
		if (y < 0 || y >= getHeight())
			return 0;
		return (byte)(map[y, x]>>24);
	}

	public static IVector2 getMouseCoords(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float sfac = ray.origin.z/ray.direction.z;
		Vector2 pos = ray.origin + sfac * ray.direction;
		return new IVector2 ((int)pos.x, (int)pos.y);
	}
	/********************/
	/*                  */
	/********************/
	
	public void setAndUpdateTileData(int x, int y, short id){
		setTileData (x, y, id);
		updateTileData(new IVector2 (x, y));
	}

	public void setTileData(int x, int y, short id){
		map [y,x] = (uint)id;
	}

	public void updateTileData(IVector2 v){
		map [v.y,v.x] &= 0x00FF;
		int[] directions = new int[] {
						Direction.TOPLEFT,
						Direction.TOP,
						Direction.TOPRIGHT,
						Direction.RIGHT,
						Direction.BOTTOMRIGHT,
						Direction.BOTTOM,
						Direction.BOTTOMLEFT,
						Direction.LEFT
				};
		bool[] b = new bool[directions.Length];
		for (int i = 0; i < directions.Length; i++)
			b[i] = isSolid(v + Direction.getDirection(directions[i]));
		map [v.y,v.x] |= (uint)Direction.packByte(directions, b) << 16;
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
		public int xTiles = 1;
		public int yTiles = 1;
	}
}
