using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TextureAtlas {
	public string name = "Texture Atlas Name";
	public Texture2D texture;
	[Range(1, 32)]
	public int width = 1;
	[Range(1, 32)]
	public int height = 1;
	[SerializeField]
	private bool folded;

	private Material material;

	public void setTexture(Texture2D texture){
		this.texture = texture;
		if (material != null)
			material.SetTexture (0, texture);
	}

	public Material getMaterial(){
		if (material == null) {
			material = new Material(Shader.Find("Custom/2D Tile Shader"));
			material.SetTexture("_Tex", texture);
			material.SetFloat("_Width", width);
			material.SetFloat("_Height", height);
		}
		return material;
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

	public Color[] tileData(int index){
		float w = pixelWidth();
		float h = pixelHeight();
		int x = index % width;
		int y = index / width;
		return texture.GetPixels ((int)(x * w), texture.height - (int)h - (int)(y * h), (int)w, (int)h);
	}

	public float pixelWidth(){
		return texture.width / width;
	}

	public float pixelHeight(){
		return texture.height / height;
	}

	public Texture2D getSubtexture(Texture2D tex, int i){
		if (texture == null) {
			Debug.Log ("Cannot get subTexture from null texture.");
			return null;
		}
		float w = texture.width / width;
		float h = texture.height / height;
		int x = i % width;
		int y = i / width;

		if (tex == null)
			tex = new Texture2D((int)w, (int)h);
		else if(tex.width != (int)w || tex.height != (int)h){
			tex = new Texture2D((int)w, (int)h);
		}
		try {
			tex.SetPixels(tileData (i));
		}
		catch (UnityException e){
			return null;
		}
		tex.filterMode = texture.filterMode;
		tex.Apply();
		return tex;
	}

	public float aspect(){
		float w = texture.width / width;
		float h = texture.height / height;
		return w / h;
	}

}
