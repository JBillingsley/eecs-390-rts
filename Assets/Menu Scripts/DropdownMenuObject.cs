using UnityEngine;
using System.Collections;

public class DropdownMenuObject : MonoBehaviour {

	public bool menuIsDown = false;
	public GameObject[] dropdownMenu;
	public DropdownMenuObject[] otherMenus;
	
	void OnMouseDown(){
		menuIsDown = !menuIsDown;
		toggleMenuDropdown();
		collapseOtherMenus();
	}
	
	// Use this for initialization
	void Start () {
		toggleMenuDropdown();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			menuIsDown = false;
			toggleMenuDropdown();
			collapseOtherMenus();
		}
	}
	
	public void toggleMenuDropdown(){
		foreach(GameObject menuItem in dropdownMenu){
			menuItem.SetActive(menuIsDown);
		}
	}
	
	private void collapseOtherMenus(){
		foreach(DropdownMenuObject menu in otherMenus){
			if(menu.menuIsDown == true && menuIsDown == true){
				menu.menuIsDown = !menuIsDown;
				menu.toggleMenuDropdown();
			}
		}
	}
}
