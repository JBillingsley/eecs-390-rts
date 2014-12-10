using UnityEngine;
using System.Collections;

public class Populator : MonoBehaviour {

	public int minx;
	public int maxx;
	public int miny;
	public int maxy;

	public MapAdjuster obj;
	public int numberOfObjects = 1;

	// Use this for initialization
	public void Populate () {
		for(int i = 0; i < numberOfObjects; i++){
			int x = Random.Range(minx,maxx);
			int y = Random.Range(miny,maxy);
			GameObject g = (Instantiate(obj.gameObject) as GameObject);
			g.transform.parent = this.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
