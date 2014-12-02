using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapAdjuster : MonoBehaviour {

	public List<TileCluster> clusters = new List<TileCluster>();

	public void activate(){
		foreach(TileCluster tc in clusters){
			activate(tc);
		}
	}

	public void activate(TileCluster tc){
		IVector2 pos = new IVector2(this.transform.position.x,this.transform.position.y);
		Map map = GameObject.FindObjectOfType<Map>();
		TileCluster t = tc.copy();
		t.x += pos.x;
		t.y += pos.y;
		map.populateCluster(t);
	}
	
}
