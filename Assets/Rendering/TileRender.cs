using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TileRender {

	public TileContext context;
	public int index;

	[SerializeField]
	private Texture2D view;

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
	private static int grey(byte val, byte lmask, byte rmask){
		return mask(val, lmask) ? (mask(val, rmask) ? 1 : 2) : (mask(val, rmask) ? 0 : 3);
	}
	
	public static bool mask(byte val, byte mask){
		return (val & mask) == mask;
	}
	
	public static int imask(byte val, byte mask){
		return (val & mask) == mask ? 1 : 0;
	}
	


/*
	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative ("view").objectReferenceValue = null;
		prop.serializedObject.ApplyModifiedProperties();
		prop.FindPropertyRelative ("view").objectReferenceValue = TileSpec.constructPreview(prop);         
		prop.serializedObject.ApplyModifiedProperties();
	}*/


}
