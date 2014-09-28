using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Tileset {
	[SerializeField]
	public string name = "Tileset Name";
	[SerializeField]
	public Texture2D texture;
	[SerializeField, Range(1, 32)]
	public int width = 1;
	[SerializeField, Range(1, 32)]
	public int height = 1;

	public Material material = new Material(Shader.Find("Custom/2D Tile Shader"));

	public Tileset(){
	
	}

	public void setTexture(Texture2D texture){
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

	public Texture2D getSubtexture(int i){
		float w = texture.width / width;
		float h = texture.height / height;
		int x = i % width;
		int y = i / width;
		Debug.Log ("Drawing ["+w+","+h+"] ["+x+","+y+"]");
		Texture2D tex = null; 
		try {
			tex = new Texture2D((int)w, (int)h);
			Color[] pixels = texture.GetPixels((int)(x*w), (int)(y*h), (int)w, (int)h);
			tex.SetPixels(pixels);
		}
		catch (UnityException e){
		}
		return tex;
	}
}
