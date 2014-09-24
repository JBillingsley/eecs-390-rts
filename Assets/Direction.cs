using UnityEngine;

public class Direction {

	public const int TOPLEFT = 0;
	public const int TOP = 1;
	public const int TOPRIGHT = 2;
	public const int RIGHT = 3;
	public const int BOTTOMRIGHT = 4;
	public const int BOTTOM = 5;
	public const int BOTTOMLEFT = 6;
	public const int LEFT = 7;

	private static readonly IVector2[] directionList = new IVector2[]{new IVector2(-1, 1), new IVector2(0, 1), new IVector2(1, 1), new IVector2(1, 0), new IVector2(1, -1), new IVector2(0, -1), new IVector2(-1, -1), new IVector2(-1, 0)}; 
	private static readonly byte[] bitMask = new byte[]{0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01};

	public static IVector2 getDirection(int direction){
		return directionList [direction];
	}
	public static IVector2[] getDirections(int[] directions){
		IVector2[] dirs = new IVector2[directions.Length];
		for (int i = 0; i < directions.Length; i++)
			dirs[i] = directionList[directions[i]];
		return dirs;
	}

	public static bool[] extractByte(int[] directions, byte b){
		bool[] dirs = new bool[directions.Length];
		for (int i = 0; i < directions.Length; i++)
			dirs[i] = (b & bitMask[directions[i]]) == bitMask[directions[i]];
		return dirs;
	}

	public static byte packByte(int[] directions, bool[] value){
		byte b = 0;
		if (directions.Length != value.Length) {
			Debug.LogError("Could not pack byte. directions and value size do not agree.");
			return 0;
		}
		for (int i = 0; i < directions.Length; i++)
			if (value[i])
				b |= bitMask [directions [i]];
		return b;
	}
}
