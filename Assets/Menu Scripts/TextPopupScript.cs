using UnityEngine;
using System.Collections;

public class TextPopupScript : MonoBehaviour {
	
	public string textToDisplay;
	public Texture textBox;
	public Texture characterIcon;
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width/5, Screen.height/5, Screen.width*3/5, Screen.height*3/5), textBox);
		// this is the second box, which is the furthest 3rd of the panel
		GUI.DrawTexture(new Rect(Screen.width*3/5, Screen.height/5, Screen.width*1/5, Screen.height*3/5), textBox);
		// this is for the character icon
		GUI.DrawTexture(new Rect(Screen.width*3/5, Screen.height/5 + Screen.width/15, Screen.width/5, Screen.width/5), characterIcon);
		GUI.Label(new Rect(Screen.width/5 + Screen.width/20, Screen.height/5 + Screen.height/20, Screen.width*3/5, Screen.height*3/5), textToDisplay);
	}
}
