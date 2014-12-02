using UnityEngine;
using System.Collections;

public class Tower : MapAdjuster {

	public int height = 0;

	public TileCluster towerCluster = new TileCluster();

	static int AIR = 0;
	static int TOWER = 11;

	// Use this for initialization
	void Start () {
		towerCluster = new TileCluster();
		towerCluster.x = -1;
		towerCluster.y = 1;
		towerCluster.width = 4;
		towerCluster.height = height;
		towerCluster.background = true;
		towerCluster.likeliness = 1;
		towerCluster.description = "the tower";
	}
	
	public void drawTower(){
		eraseTower();
		towerCluster.tileType = TOWER;
		draw();
	}

	public void eraseTower(){
		towerCluster.tileType = AIR;
		draw ();
	}

	public void increaseHeight(){
		height++;
		towerCluster.height = height;
		drawTower();
	}

	public void decreaseHeight(){
		eraseTower();
		height--;
		towerCluster.height = height;
		drawTower();
	}

	public void draw(){
		Map m = GameObject.FindObjectOfType<Map>();
		IVector2 pos = new IVector2(this.transform.position.x,this.transform.position.y);
		TileCluster t = towerCluster.copy();
		for(int x = t.x; x < t.x + t.width; x++){
			for(int y = t.y; y < t.y + t.height; y++){
				if(Random.value < t.likeliness){
					m.setTile(new IVector2(pos.x + x, pos.y + y),(byte)0,(byte)towerCluster.tileType);
				}
			}
		}
	}
}
