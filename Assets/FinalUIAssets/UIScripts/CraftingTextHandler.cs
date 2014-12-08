using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingTextHandler : MonoBehaviour {

	public Text target;
	public string itemName;
	public string itemDescription;
	
	
	
	public CraftingReqs requirements;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		string resources = "";
		
		foreach(Reqs req in requirements.requirements){
			resources += " " + req.type + " " + getAmount(req.type) + "/" + req.amount;
		}
		target.text = itemName + "\n" + 
					  itemDescription + "\n" +
					  "Resources required:" + resources;
	}
	
	public string getAmount(Element type){
		return "" + InventroyManager.instance.getCount(type);
	}
}
