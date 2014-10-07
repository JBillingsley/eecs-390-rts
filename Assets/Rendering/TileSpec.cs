using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TileSpec {

	public string name = "Tile Name";
	public TileContext context;
	public int index;
	public bool solid;

	[SerializeField]
	private bool folded;
	[SerializeField]
	private Texture2D view;


	public static Texture2D constructPreview(TileSpec spec, TextureAtlas atlas){
		if (spec.view != null)
			Texture2D.Destroy(spec.view);
		int w = (int)atlas.pixelWidth();
		int h = (int)atlas.pixelHeight();
		switch (spec.context) {
		case TileContext.None:
			spec.view = fillTexture(spec.view, 1, 1, atlas, new int[]{spec.index});
			return spec.view;
		case TileContext.PartialContext:
			spec.view = fillTexture(spec.view, 3, 3, atlas, new int[]{
				spec.index, spec.index + 1, spec.index + 2, 
				spec.index + 4, spec.index + 5, spec.index + 6, 
				spec.index + 8, spec.index + 9, spec.index + 10});
			return spec.view;
		case TileContext.FullContext:
			spec.view = fillTexture(spec.view, 5, 5, atlas, new int[]{
				spec.index + 4, spec.index + 34, spec.index + 1, spec.index + 33, spec.index + 6, 
				spec.index + 36, spec.index + 22, spec.index + 11, spec.index + 20, spec.index + 39, 
				spec.index + 8, spec.index + 25, spec.index + 9, spec.index + 25, spec.index + 10,
				spec.index + 40, spec.index + 6, spec.index + 11, spec.index + 4, spec.index + 43,
				spec.index + 20, spec.index + 46, spec.index + 17, spec.index + 45, spec.index + 22});
			return spec.view;
		case TileContext.Slope:
			return spec.view;
		}
		return null;
	}

	private static Texture2D fillTexture(Texture2D texture, int width, int height, TextureAtlas atlas, int[] indices){
		int w = (int)atlas.pixelWidth();
		int h = (int)atlas.pixelHeight();
		texture = ensureSize(texture, width * w, height * h);
		for (int i = 0; i < width * height; i++)
			texture.SetPixels(i % width, i / width, w, h, atlas.tileData(indices[i]));
		texture.Apply ();
		return texture;
	}

	private static Texture2D ensureSize(Texture2D texture, int width, int height){
		if (texture == null) {
			texture = new Texture2D(width, height, TextureFormat.BGRA32, false);
		}
		else if (texture.width != width || texture.height != height) {
			Texture2D.Destroy(texture);
			texture = new Texture2D(width, height, TextureFormat.BGRA32, false);
		}
		return texture;
	}

	public static float previewSize(TileContext context){
		switch (context) {
		case TileContext.None:
			return 16;
		case TileContext.PartialContext:
			return 48;
		case TileContext.FullContext:
			return 80;
		case TileContext.Slope:
			return 48;
		}
		return 0;
	}

	
	public int getTexID(byte adj){
		switch (context){
		case TileContext.PartialContext:
			return index + getPartialContext(adj);
		case TileContext.FullContext:
			return index + getFullContext (adj);
		}
		return index;
	}
	
	public static int getPartialContext(byte adj){
		return 4 * grey(adj, 0x40, 0x04) + grey(adj, 0x01, 0x10);
	}

	public static int getFullContext(byte adj){
		int id = 8 * grey(adj, 0x40, 0x04) + grey(adj, 0x01, 0x10);
		switch (id) {
		case 0:
			return id + 4 * imask(adj, 0x08);
		case 2:
			return id + 4 * imask(adj, 0x02);
		case 16:
			return id + 4 * imask(adj, 0x20);
		case 18:
			return id + 4 * imask(adj, 0x80);
		case 1:
			if (mask (adj, 0x0A))
				return id + 4;
			return id + 32 * imask(adj, 0x08) + 33 * imask(adj, 0x02);
		case 8:
			if (mask (adj, 0x28))
				return id + 4;
			return id + 28 * imask(adj, 0x20) + 32 * imask(adj, 0x08);
		case 10:
			if (mask (adj, 0x82))
				return id + 4;
			return id + 29 * imask(adj, 0x80) + 33 * imask(adj, 0x02);
		case 17:
			if (mask (adj, 0xA0))
				return id + 4;
			return id + 29 * imask(adj, 0x80) + 28 * imask(adj, 0x20);
		case 9:
			if (mask (adj, 0xAA))
				return id + 4;
			if (mask (adj, 0x88))
				return id + 20 + 8 * imask (adj, 0x20) + 13 * imask (adj, 0x02);
			if (mask (adj, 0x22))
				return id + 19 + 10 * imask (adj, 0x80) + 13 * imask (adj, 0x08);
			int delta = -2 * imask (adj, 0x0A) + 6 * imask (adj, 0x82) + 14 * imask (adj, 0xA0) + 22 * imask (adj, 0x28);
			if (delta != 0)
				return id + delta;
			return id + 23 * imask (adj, 0x80) + 26 * imask (adj, 0x20) + 38 * imask (adj, 0x08) + 35 * imask (adj, 0x02);
		}
		return id;
	}

	public void getCollisionIndices(int i, int off, byte adj, List<int> list){
		if (context == TileContext.None){
			list.Add(i + 1);
			list.Add(i + 1 + off);
			list.Add(i + 2);
			list.Add(i + 1 + off);
			list.Add(i + 2 + off);
			list.Add(i + 2);
			
			list.Add(i + 2);
			list.Add(i + 2 + off);
			list.Add(i + 3);
			list.Add(i + 2 + off);
			list.Add(i + 3 + off);
			list.Add(i + 3);
			
			list.Add(i + 3);
			list.Add(i + 3 + off);
			list.Add(i + 0);
			list.Add(i + 3 + off);
			list.Add(i + 0 + off);
			list.Add(i + 0);
			
			list.Add(i + 0);
			list.Add(i + 0 + off);
			list.Add(i + 1);
			list.Add(i + 0 + off);
			list.Add(i + 1 + off);
			list.Add(i + 1);
			return;
		}
		if (mask (adj, 0x40)){
			list.Add(i + 1);
			list.Add(i + 1 + off);
			list.Add(i + 2);
			list.Add(i + 1 + off);
			list.Add(i + 2 + off);
			list.Add(i + 2);
		}
		if (mask (adj, 0x10)){
			list.Add(i + 2);
			list.Add(i + 2 + off);
			list.Add(i + 3);
			list.Add(i + 2 + off);
			list.Add(i + 3 + off);
			list.Add(i + 3);
		}
		if (mask (adj, 0x04)){
			list.Add(i + 3);
			list.Add(i + 3 + off);
			list.Add(i + 0);
			list.Add(i + 3 + off);
			list.Add(i + 0 + off);
			list.Add(i + 0);
		}
		if (mask (adj, 0x01)){
			list.Add(i + 0);
			list.Add(i + 0 + off);
			list.Add(i + 1);
			list.Add(i + 0 + off);
			list.Add(i + 1 + off);
			list.Add(i + 1);
		}
		return;	}
	
	private static int grey(byte val, byte lmask, byte rmask){
		return mask(val, lmask) ? (mask(val, rmask) ? 1 : 2) : (mask(val, rmask) ? 0 : 3);
	}
	
	public static bool mask(byte val, byte mask){
		return (val & mask) == mask;
	}
	
	public static int imask(byte val, byte mask){
		return (val & mask) == mask ? 1 : 0;
	}


	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative("name").stringValue = "Tile Name";         
		prop.serializedObject.ApplyModifiedProperties();
	}


}
