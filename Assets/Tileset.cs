using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Tileset {
	[SerializeField]
	public string name = "Tileset Name";
	[SerializeField]
	public Texture texture;
	[SerializeField, Range(1, 32)]
	public int width = 1;
	[SerializeField, Range(1, 32)]
	public int height = 1;

	public Material material = new Material(Shader.Find("Custom/2D Tile Shader"));

	public Tileset(){
	
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
