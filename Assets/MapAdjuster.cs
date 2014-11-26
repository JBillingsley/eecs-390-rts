using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapAdjuster : MonoBehaviour {

	public List<TileCluster> clusters = new List<TileCluster>();
	private Map map;

	public void activate(){
		map = GameObject.FindObjectOfType<Map>();
		IVector2 pos = new IVector2(this.transform.position.x,this.transform.position.y);
		foreach(TileCluster tc in clusters){
			TileCluster t = tc;
			t.x += pos.x;
			t.y += pos.y;
			map.populateCluster(t);
		}
	}
	
}
