﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

	public static readonly Tile[] tiles = new Tile[32];

	public static readonly Tile air = new Tile (0, false);
	public static readonly Tile dirt = new Tile (1, true, true);
	public static readonly Tile thing = new Tile (49, true, true);
	
	public static readonly short defaultTile = 1;


	public readonly short id;
	public readonly int texID;
	public readonly bool solid;
	public readonly bool context;

	private static short i = 0;

	private Tile(int texID, bool solid) : this(texID, solid, false){}

	private Tile(int texID, bool solid, bool context){
		this.texID = texID;
		this.solid = solid;
		this.context = context;
		this.id = i;
		tiles[i++] = this;
	}

	public int getTexID(byte adj){
		if (!context)
			return texID;
		int id = 8 * grey(adj, 0x40, 0x04) + grey(adj, 0x01, 0x10);
		switch (id) {
			case 0:
				return texID + id + 4 * imask(adj, 0x08);
			case 2:
				return texID + id + 4 * imask(adj, 0x02);
			case 16:
				return texID + id + 4 * imask(adj, 0x20);
			case 18:
				return texID + id + 4 * imask(adj, 0x80);
			case 1:
				if (mask (adj, 0x0A))
					return texID + id + 4;
				return texID + id + 32 * imask(adj, 0x08) + 33 * imask(adj, 0x02);
			case 8:
				if (mask (adj, 0x28))
					return texID + id + 4;
				return texID + id + 28 * imask(adj, 0x20) + 32 * imask(adj, 0x08);
			case 10:
				if (mask (adj, 0x82))
					return texID + id + 4;
				return texID + id + 29 * imask(adj, 0x80) + 33 * imask(adj, 0x02);
			case 17:
				if (mask (adj, 0xA0))
					return texID + id + 4;
				return texID + id + 29 * imask(adj, 0x80) + 28 * imask(adj, 0x20);
			case 9:
				if (mask (adj, 0xAA))
					return texID + id + 4;
				if (mask (adj, 0x88))
					return texID + id + 20 + 8 * imask (adj, 0x20) + 13 * imask (adj, 0x02);
				if (mask (adj, 0x22))
					return texID + id + 19 + 10 * imask (adj, 0x80) + 13 * imask (adj, 0x08);
				int delta = -2 * imask (adj, 0x0A) + 6 * imask (adj, 0x82) + 14 * imask (adj, 0xA0) + 22 * imask (adj, 0x28);
				if (delta != 0)
					return texID + id + delta;
				return texID + id + 23 * imask (adj, 0x80) + 26 * imask (adj, 0x20) + 38 * imask (adj, 0x08) + 35 * imask (adj, 0x02);
		}
		return texID + id;
	}

	public void getCollisionIndices(int i, int off, byte adj, List<int> list){
		if (!context){
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
//All American Furry Encounter

	private int grey(byte val, byte lmask, byte rmask){
		return mask(val, lmask) ? (mask(val, rmask) ? 1 : 2) : (mask(val, rmask) ? 0 : 3);
	}

	public bool mask(byte val, byte mask){
		return (val & mask) == mask;
	}

	public int imask(byte val, byte mask){
		return (val & mask) == mask ? 1 : 0;
	}
	
}
