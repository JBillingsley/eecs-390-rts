using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Tileset : Object {

	public Texture texture;
	public int width;
	public int height;


	public static Material getMaterial(Tileset t){
		Material m = new Material(Shader.Find("Custom/2D Tile Shader"));
		m.SetTexture (0, t.texture);
		m.SetFloat("_Width", t.width);
		m.SetFloat("_Height", t.height);
		return m;
	}


	public void setTexture(Texture texture){
		this.texture = texture;
	}

	public void setWidth(int width){
		this.width = width;
	}
	
	public void setHeight(int height){
		this.height = height;
	}
}
