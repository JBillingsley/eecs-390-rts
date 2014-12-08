using UnityEngine;
using System.Collections;

public class Tower : MapAdjuster {

	public int height = 0;
	public TowerType type = TowerType.WOOD;
	
	public enum TowerType {WOOD,CLAY,STONE};

	public TileCluster towerCluster = new TileCluster();

	static int AIR = 0;
	static string TOWER_STONE = "Stone Tower";
	static string TOWER_CLAY = "Clay Tower";
	static string TOWER_WOOD = "Wood Tower";

	// Use this for initialization
	void Start () {
		towerCluster = new TileCluster();
		towerCluster.x = 1;
		towerCluster.y = 1;
		towerCluster.width = 4;
		towerCluster.height = 0;
		towerCluster.background = true;
		towerCluster.likeliness = 1;
		towerCluster.description = "the tower";
	}
	
	public void drawTower(){
		eraseTower();
		towerCluster.tileType = towerType();
		draw();
	}

	public void eraseTower(){
		towerCluster.tileType = "air";
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
			for(int y = t.y; y < t.y + t.height + 2; y++){
				if(Random.value < t.likeliness){
					m.setTile(new IVector2(pos.x + x, pos.y + y),(byte)0,(byte)TileSpecList.getTileSpecInt(towerCluster.tileType));
				}
			}
		}
	}
	
	public string towerType(){
		switch(type){
		case TowerType.WOOD:
			return TOWER_WOOD;
		case TowerType.CLAY:
			return TOWER_CLAY;
		case TowerType.STONE:
			return TOWER_STONE;
		}
		return TOWER_WOOD;
	}
}
