using UnityEngine;
using System.Collections;

public class CraftingBarActions : MonoBehaviour {

	public Texture panelTexture;
	public Texture buttonTexure;
	public Texture[] iconTexture;
	public Button[] buttons;
	//public float panelWidth;
	public float panelHeight;
	public float padding;
	private string amount;
	public Element[] type;
	
	void OnGUI(){
		panelHeight = (Screen.width/1.618f)/20;
		//	panelWidth = Screen.width/1.5f;
		GUI.DrawTexture(new Rect((Screen.width/2 - ((panelHeight + padding) * (iconTexture.Length))/2), Screen.height - panelHeight, ((panelHeight + padding) * (iconTexture.Length)) + padding, panelHeight), panelTexture);
		for(int i = 0; i < iconTexture.Length; i++){
			float buttonOffset = ((panelHeight + padding)*i) + padding;
			if(GUI.Button(new Rect(((Screen.width/2 - ((panelHeight + padding) * (iconTexture.Length))/2)) + buttonOffset, Screen.height - panelHeight, panelHeight, panelHeight), iconTexture[i])){
				buttonAction();
			}
			amount = calculateAmount(i);
			GUI.Label(new Rect((((Screen.width/2 - ((panelHeight + padding) * (iconTexture.Length))/2)) + buttonOffset) + (3*panelHeight/4) - 5, Screen.height - panelHeight, panelHeight/3, panelHeight/2), amount);
		}
	}
	
	protected string calculateAmount(int i){
		return "" + InventroyManager.instance.getCount(type[i]);
	}
	
	protected void buttonAction(){
		
	}
}
