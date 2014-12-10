using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public float startingOffset;
	private float lastEnemyTime;
	public float enemySpawnTime;
	public GenericFactory factory;

	public Vector2 destination;

	// Use this for initialization
	void Start () {
		lastEnemyTime = Time.timeSinceLevelLoad - startingOffset;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - lastEnemyTime > enemySpawnTime){
			Vector2 d = new Vector2(Random.Range(0,95),Random.Range(1,60));
			lastEnemyTime = Time.timeSinceLevelLoad * Util.randomSpread();
			Enemy e = factory.spawnObject().GetComponent<Enemy>();
			e.gameObject.SetActive(true);
			e.transform.position = this.transform.position;
			e.destination = d;
		}
	}
}
