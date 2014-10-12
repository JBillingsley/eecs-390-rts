using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitMesh {

	public static UnitMesh inst = new UnitMesh();

	public Mesh leftMesh;
	public Mesh rightMesh;

	private UnitMesh(){
	}

}
