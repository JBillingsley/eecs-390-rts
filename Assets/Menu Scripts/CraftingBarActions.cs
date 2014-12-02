using UnityEngine;
using System.Collections;

public class CraftingBarActions : MonoBehaviour {

	public Texture panelTexture;
	public Texture[] iconTexture;
	public float panelHeight;
	public float padding;
	public GenericFactory unitSpawner;
	private string amount;

	private Tower tower;

	int DIRT_GOLEM_COST = 10;
	int ROCK_GOLEM_COST = 10;
	int SUPER_GOLEM_COST = 10;

	int DIRT_TOWER_COST = 10;
	int CLAY_TOWER_COST = 10;
	int STONE_TOWER_COST = 10;
	
	void OnGUI(){
		panelHeight = (Screen.width/1.618f)/20;
		//	panelWidth = Screen.width/1.5f;
		GUI.DrawTexture(new Rect((Screen.width/2 - ((panelHeight + padding) * (iconTexture.Length))/2), Screen.height - panelHeight, ((panelHeight + padding) * (iconTexture.Length)) + padding, panelHeight), panelTexture);
		for(int i = 0; i < iconTexture.Length; i++){
			float buttonOffset = ((panelHeight + padding)*i) + padding;
			if(GUI.Button(new Rect(((Screen.width/2 - ((panelHeight + padding) * (iconTexture.Length))/2)) + buttonOffset, Screen.height - panelHeight, panelHeight, panelHeight), iconTexture[i])){
				buttonAction(i);
			}
			amount = calculateAmount(i);
			GUI.Label(new Rect((((Screen.width/2 - ((panelHeight + padding) * (iconTexture.Length))/2)) + buttonOffset) + (3*panelHeight/4) - 5, Screen.height - panelHeight, panelHeight/3, panelHeight/2), amount);
		}
	}
	
	protected string calculateAmount(int i){
		switch (i){
		case 0:
			return "" + InventroyManager.instance.getCount(Element.DIRT) / DIRT_GOLEM_COST;
			break;
		case 1:
			return "" + 0;
			break;
		case 2:
			return "" + 0;
			break;
		case 3:
			return "" + InventroyManager.instance.getCount(Element.DIRT) / DIRT_TOWER_COST;
			break;
		case 4:
			return "" + 0;
			break;
		case 5:
			return "" + 0;
			break;
		}
		return "" + 0;
	}
	
	protected void buttonAction(int i){
		if(tower == null){
			tower = GameObject.FindObjectOfType<Tower>();
		}
		switch (i){
		case 0: //Golem 1
			if(InventroyManager.instance.getCount(Element.DIRT) >= DIRT_GOLEM_COST){
				InventroyManager.instance.removeFromInventory(Element.DIRT, DIRT_GOLEM_COST);
				// make a CUTIE
				unitSpawner.spawnObject();
			}
			break;
		case 1: //Golem 2
			
			break;
		case 2: //Golem 3
			//return ;
			break;
		case 3: //Tower 1
			if(tower != null && InventroyManager.instance.getCount(Element.DIRT) >= DIRT_TOWER_COST){
				InventroyManager.instance.removeFromInventory(Element.DIRT, DIRT_TOWER_COST);			
				tower.increaseHeight();
			}
			break;
		case 4: //Tower 2
			//
			break;
		case 5: //Tower 3
			//
			break;
		}
	}
}
