using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Tileset {

	public string name;
	public Texture texture;
	public int width;
	public int height;

	public Material material;

	public void Awake(){
		material = new Material(Shader.Find("Custom/2D Tile Shader"));
	}

	public void setTexture(Texture texture){
		this.texture = texture;
		if (material != null)
			material.SetTexture (0, texture);
	}

	public void setWidth(int width){
		this.width = width;
		if (material != null)
			material.SetFloat("_Width", width);
	}
	
	public void setHeight(int height){
		this.height = height;
		if (material != null)
			material.SetFloat("_Height", height);
	}
}
