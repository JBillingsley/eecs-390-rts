using UnityEngine;
using System.Collections;

public class DropdownMenuObject : MonoBehaviour {

	public bool menuIsDown = false;
	public GameObject[] dropdownMenu;
	
	void OnMouseDown(){
		menuIsDown = !menuIsDown;
		toggleMenuDropdown();
	}
	
	// Use this for initialization
	void Start () {
		toggleMenuDropdown();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void toggleMenuDropdown(){
		foreach(GameObject menuItem in dropdownMenu){
			menuItem.SetActive(menuIsDown);
		}
	}
}
