using UnityEngine;
using System.Collections;

public class TextPopupScript : MonoBehaviour {
	
	public string textToDisplay;
	public Texture textBox;
	public Texture characterIcon;
	public GUIStyle textStyle;
	public GUIStyle buttonStyle;
	public string leftButtonContext;
	public string rightButtonContext;
	private string initialText;
	Vector2 position;
	
	
	void Start(){
		leftButtonContext = "Trade";
		rightButtonContext = "Talk";
		initialText = textToDisplay;
	}
	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width/5, Screen.height/5, Screen.width*3/5, Screen.height*3/5), textBox);
		// this is the second box, which is the furthest 3rd of the panel
		GUI.DrawTexture(new Rect(Screen.width*3/5, Screen.height/5, Screen.width*1/5, Screen.height*3/5), textBox);
		// this is for the character icon
		GUI.DrawTexture(new Rect(Screen.width*3/5, Screen.height/5 + Screen.width/15, Screen.width/5, Screen.width/5), characterIcon);
		
		GUI.Label(new Rect(Screen.width/5 + Screen.width/20, Screen.height/5 + Screen.height/20, Screen.width*3/5, Screen.height*3/5), textToDisplay, textStyle);
		
		// This will initiate the trade sequence
		if(GUI.Button(new Rect(new Rect(Screen.width/5 + Screen.width/20, Screen.height/5 + Screen.height*1/2, Screen.width/12, Screen.height/15)), leftButtonContext,buttonStyle)){
			if(leftButtonContext.Equals("Trade")){
				tradeOption("Deal");
			} else if(leftButtonContext.Equals("Deal")){
				// disable menu and do a thing maybe
				tradeOption("Leave");
				// this is for the talk option
			} else if(leftButtonContext.Equals("Leave")){
				// turn off the panel
				tradeOption("");
				enabled = false;
			} else {
				// disable menu and do a different thing
				tradeOption("");
			}
		}
		
		if(GUI.Button(new Rect(new Rect(Screen.width/5 + Screen.width/5, Screen.height/5 + Screen.height*1/2, Screen.width/12, Screen.height/15)), rightButtonContext,buttonStyle)){
			if(rightButtonContext.Equals("Talk")){
				talkOption("Help");
			} else if(rightButtonContext.Equals("Help")){
				// go back to talk option
				talkOption("Agree");
			} else {
				// i actually am not sure...
				talkOption("");
			}
		}
	}
	
	private void tradeOption(string message){
		// we need to set both button contexts
		// we need to change the label text
		// we need to maybe to other things
		leftButtonContext = message;
		if(leftButtonContext.Equals("Deal")){
			rightButtonContext = "No Deal";
			textToDisplay = "We have been short on our crops this season. "+ 
							"If you could manage to spare 20 units of Wood and 10 Mushrooms "+
							"We could give you some of the rare metals we have stored away from the Mole People.";
		} else if(leftButtonContext.Equals("Leave")){
			rightButtonContext = "Repeat";
			textToDisplay = "Wonderful! Is there anything else I can help you with?";
			// set a quest thing for the quest thing
		} else {
			// uuhhhh
			textToDisplay = initialText;
			leftButtonContext = "Trade";
			rightButtonContext = "Talk";
		}
	}
	
	private void talkOption(string message){
		// we need to set both button contexts
		// we need to change the label text
		// we need to maybe to other things
		rightButtonContext = message;
		if(rightButtonContext.Equals("Help")){
			leftButtonContext = "Back";
			textToDisplay = "Our village is in need of some rare resources. "+
							"Would you be so kind as to provide us with: "+
							"20 units of Lumber, 40 units of Clay, and 10 units of Metal? \n"+
							"We will make it worth your while!";
		} else if(rightButtonContext.Equals("Agree")){
			tradeOption("Leave");
		} else {
			textToDisplay = initialText;
			leftButtonContext = "Trade";
			rightButtonContext = "Talk";
		}
	}
}
