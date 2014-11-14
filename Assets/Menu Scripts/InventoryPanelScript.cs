using UnityEngine;
using System.Collections;

public class InventoryPanelScript : MonoBehaviour {

	public Texture panelTexture;
	public Texture buttonTexure;
	public Texture[] iconTexture;
	public Button[] buttons;
	public float panelWidth;
	public float panelHeight;
	public float padding;
	
	void OnGUI(){
		panelHeight = (Screen.width/1.618f)/12;
		panelWidth = Screen.width/1.5f;
		GUI.DrawTexture(new Rect(((panelHeight + padding) * (iconTexture.Length)), Screen.height - panelHeight, ((panelHeight + padding) * (iconTexture.Length)) + padding, panelHeight), panelTexture);
		for(int i = 0; i < iconTexture.Length; i++){
			float buttonOffset = ((panelHeight + padding)*i) + padding;
			if(GUI.Button(new Rect(((panelHeight + padding) * (iconTexture.Length)) + buttonOffset, Screen.height - panelHeight, panelHeight, panelHeight), iconTexture[i])){
				
			}
			// TODO: change this to the inventory numbers
			string content = "0";
			GUI.Label(new Rect((((panelHeight + padding) * (iconTexture.Length)) + buttonOffset) + (3*panelHeight/4), Screen.height - panelHeight, panelHeight/4, panelHeight/2), content);
		}
	}
}
