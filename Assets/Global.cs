using UnityEngine;
using System.Collections;

[System.Serializable]
public class Global : MonoBehaviour{
	public static Global global = new Global();

	public Tileset[] tilesets = new Tileset[1];

}
