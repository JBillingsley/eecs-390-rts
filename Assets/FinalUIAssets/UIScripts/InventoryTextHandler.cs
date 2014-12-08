using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryTextHandler : MonoBehaviour {
	
	public Text text;
	public string ItemName;
	public string ItemDescription;
	public string ItemUses;
	public Element type;
	string ItemAmount;
	
	// Use this for initialization
	void Start () {
		ItemAmount = "Amount: ";
	}
	
	// Update is called once per frame
	void Update () {
		text.text = ItemName + "\n" +
					ItemAmount + getItemAmount(type) + "\n" +
					ItemDescription + "\n" + 
					ItemUses;
			
	}
	
	public string getItemAmount(Element type){
		return "" + InventroyManager.instance.getCount(type);
	}
}
