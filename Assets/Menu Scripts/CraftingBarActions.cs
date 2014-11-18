using UnityEngine;
using System.Collections;

public class CraftingBarActions : MonoBehaviour {

	public Texture panelTexture;
	public Texture[] iconTexture;
	public float panelHeight;
	public float padding;
	private string amount;
	
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
			return "" + InventroyManager.instance.getCount(Element.DIRT) % 10;
			break;
		case 1:
			return "" + 0;
			break;
		case 2:
			return "" + 0;
			break;
		case 3:
			return "" + 0;
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
		switch (i){
		case 0:
			if(InventroyManager.instance.getCount(Element.DIRT) >= 10){
				
			}
			break;
		case 1:
			
			break;
		case 2:
			//return ;
			break;
		case 3:
			//return ;
			break;
		case 4:
			//
			break;
		case 5:
			//
			break;
		}
	}
}
