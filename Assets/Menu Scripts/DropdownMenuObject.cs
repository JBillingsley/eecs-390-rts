using UnityEngine;
using System.Collections;

public class DropdownMenuObject : MonoBehaviour {

	public bool menuIsDown = false;
	public GameObject[] dropdownMenu;
	public DropdownMenuObject[] otherMenus;
	
	void OnMouseDown(){
		menuIsDown = !menuIsDown;
		foreach(DropdownMenuObject menu in otherMenus){
			if(menu.menuIsDown == true && menuIsDown == true){
					menu.menuIsDown = !menuIsDown;
					menu.toggleMenuDropdown();
			}
		}
		toggleMenuDropdown();
	}
	
	// Use this for initialization
	void Start () {
		toggleMenuDropdown();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void toggleMenuDropdown(){
		foreach(GameObject menuItem in dropdownMenu){
			menuItem.SetActive(menuIsDown);
		}
	}
}
