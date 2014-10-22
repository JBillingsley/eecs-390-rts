using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public List<GenericFactory> factories;

	public List<GameObject> enemies;

	public Enemy enemy;

	// Use this for initialization
	void Start () {
		factories = new List<GenericFactory>();
		factories.Add(new GenericFactory(enemy.gameObject));
	}

	//Get the enemy nearest the given point
	public Enemy getNearestEnemy(Vector2 v){
		updateList();
		Enemy e = (Enemy)enemies[0].GetComponent<Enemy>();
		float minDist;

		Vector2 pos = new Vector2(e.transform.position.x,e.transform.position.y);
		minDist = (pos-v).magnitude;

		//Go through each active object.
		foreach(GameObject g in enemies){
			pos = new Vector2(g.transform.position.x,g.transform.position.y);
			if((pos - v).magnitude < minDist){
				e = g.GetComponent<Enemy>();
			}
		}
		return e;
	}

	//Update the list of enemies.
	private void updateList(){
		enemies = new List<GameObject>();
		foreach(GenericFactory f in factories){
			enemies.AddRange(f.getActiveObjects());
		}
	}
}
