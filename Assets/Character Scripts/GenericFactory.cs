using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericFactory : MonoBehaviour{

	public GameObject spawnedObject;

	private List<GameObject> activeObjects;
	private Queue<GameObject> inactiveObjects;

	// Use this for initialization
	void Start () {
		activeObjects = new List<GameObject>();
		inactiveObjects = new Queue<GameObject>();
		InvokeRepeating("recycleObjects",1,5);
	}
	
	//Recycles inactive objects
	void recycleObjects(){
		List<GameObject> newList = new List<GameObject>();
		//Go through each object
		foreach(GameObject go in activeObjects){
			//if not null
			if(go != null){
				//Give inactive objects to the inactive list
				if(!go.activeSelf){
					inactiveObjects.Enqueue (go);
				}
				//Keep everything else in the active object
				else{
					newList.Add(go);
				}
			}
		}
		activeObjects = newList;
	}

	public GameObject spawnObject(){
		GameObject returnObject;
		//Check if you should pull from used objects...
		if(inactiveObjects.Count > 0){
			returnObject = inactiveObjects.Dequeue();
		}
		//...or make a new one.
		else{
			returnObject = (Instantiate(spawnedObject) as GameObject);
		}
		activeObjects.Add (returnObject);
		returnObject.SetActive(true);
		return returnObject;
	}
}
