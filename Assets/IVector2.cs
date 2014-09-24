using UnityEngine;
using System;

[Serializable]
public class IVector2 {

	public readonly int x, y;

	public IVector2(int x, int y){
		this.x = x;
		this.y = y;
	}

	/*****/

	public static implicit operator Vector2(IVector2 v){
		return new Vector2 (v.x, v.y);
	}

	public static implicit operator IVector2(Vector2 v){
		return new IVector2 ((int)v.x, (int)v.y);
	}

	/*****/

	public static IVector2 operator +(IVector2 v1, IVector2 v2){
		return new IVector2 (v1.x + v2.x, v1.y + v2.y);
	}

	public static IVector2 operator -(IVector2 v1, IVector2 v2){
		return new IVector2 (v1.x - v2.x, v1.y - v2.y);
	}

	public static IVector2 operator *(IVector2 v1, float f){
		return new IVector2 ((int)(v1.x * f), (int)(v1.y * f));
	}

	public static IVector2 operator *(IVector2 v1, int i){
		return new IVector2 (v1.x * i, v1.y * i);
	}
	
	public static IVector2 operator /(IVector2 v1, float f){
		return new IVector2 ((int)(v1.x / f), (int)(v1.y / f));
	}
	
	public static IVector2 operator /(IVector2 v1, int i){
		return new IVector2 (v1.x / i, v1.y / i);
	}
}
