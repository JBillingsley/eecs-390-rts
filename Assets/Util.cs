using UnityEngine;
using System.Collections;

public class Util{

	public static string pair(int x, int y){
		return " [" + x + ", " + y + "]";
	}

	public static float randomSpread(){
		return randomSpread(.1f);
	}

	public static float randomSpread(float spread){
		return Random.Range(1 - spread,1 + spread);
	}
}
