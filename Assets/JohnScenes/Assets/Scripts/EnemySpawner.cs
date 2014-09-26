using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

public float lastEnemyTime;
public float enemySpawnTime;
public GameObject enemy;

	// Use this for initialization
	void Start () {
		lastEnemyTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - lastEnemyTime > enemySpawnTime){
			lastEnemyTime = Time.timeSinceLevelLoad;
			 Instantiate(enemy, transform.position, Quaternion.identity);
		}
	}
}
