using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour{
	private static readonly IVector2[] directions = new IVector2[]{new IVector2(-1, 1), new IVector2(0, 1), new IVector2(1, 1), new IVector2(1, 0), new IVector2(1, -1), new IVector2(0, -1), new IVector2(-1, -1), new IVector2(-1, 0)}; 
	private static readonly uint[] adjBits = new uint[]{0x00800000, 0x00400000, 0x00200000, 0x00100000, 0x00080000, 0x00040000, 0x00020000, 0x00010000};
	private static readonly uint[] pathBits = new uint[]{0x80000000, 0x40000000, 0x20000000, 0x10000000, 0x08000000, 0x04000000, 0x02000000, 0x01000000};
	
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
				updateTile(x, y);
	}

	/********************/
	/*                  */
	/********************/

	public Tile getTile(int x, int y){
		return Tile.tiles[getTileID(x, y)];
	}
	public short getTileID(int x, int y){
		if (x < 0 || x >= getWidth())
			return Tile.defaultTile;
		if (y < 0 || y >= getHeight())
			return Tile.defaultTile;
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

	/********************/
	/*                  */
	/********************/
	
	public void setAndUpdateTile(int x, int y, short id){
		setTile (x, y, id);
		updateTile(x, y);
	}

	public void setTile(int x, int y, short id){
		map [y,x] = (uint)id;
	}

	public void updateTile(int x, int y){
		map [y,x] &= 0x00FF;
		for (int i = 0; i < 8; i++) {
			if(getTile(x + directions[i].x, y + directions[i].y).solid)
				map [y,x] |= adjBits[i];
			else if(getTile(x + directions[i].x, y + directions[i].y - 1).solid)
				map [y,x] |= pathBits[i];
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
		public int xTiles = 1;
		public int yTiles = 1;
	}

	private class IVector2{
		public int x, y;
		public IVector2(int x, int y){
			this.x=x;
			this.y=y;
		}
	}
}
