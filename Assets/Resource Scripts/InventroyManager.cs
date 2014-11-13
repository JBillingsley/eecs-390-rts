using UnityEngine;
using System.Collections;

public class InventroyManager : MonoBehaviour {

	private static InventroyManager _instance;
	private int[] inventory;
	// Use this for initialization
	void Start () {
		_instance = this;
		inventory = new int[(int)Element.length];
	}
	
	public static InventroyManager instance {
		get {
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<InventroyManager>();
			}
			return _instance;
		}
	}
	
	public void addToInventory(Element item){
		Debug.Log ("inc element: " + item);
		inventory[(int)item] += 1;
	}
	
	public void removeFromInventory(Element item, int amount){
		inventory[(int)item] -= amount;
	}
	
	public int getCount(Element type){
		return inventory[(int)type];
	}
}