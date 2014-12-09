using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Reqs {
	public Element type;
	public int amount;
}

public class CraftingReqs : MonoBehaviour {
	
	public List<Reqs> requirements;
	public GenericFactory unitSpawner;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void buttonAction(){
		if(updateResources() > 0){
			makeDude();
		}
	}
	
	public void makeDude(){
		foreach(Reqs req in requirements){
			InventroyManager.instance.removeFromInventory(req.type, req.amount);
		}
		// call the spawn dude function
		if(unitSpawner != null){
			unitSpawner.spawnObject();
			Debug.Log("Spawned a cutie");
		}
	}
	
	public int updateResources(){
		int[] reqAmts = new int[requirements.Count];
		
		for(int i = 0; i < requirements.Count; i++){
			reqAmts[i] = calcRequirementAmt(requirements[i].type);
		}
		return findMin(reqAmts);
	}
	
	public int findMin(int[] values){
		int min = 0;
		foreach(int i in values){
			if(i < min){
				min = i;
			}
		}
		return min;
	}
	
	public int calcRequirementAmt(Element type){
		foreach(Reqs req in requirements){
			if(req.type == type){
				return InventroyManager.instance.getCount(type) / req.amount;
			}
		}
		// if this statement is reached the element is not a requirement
		return -1;
	}
}
