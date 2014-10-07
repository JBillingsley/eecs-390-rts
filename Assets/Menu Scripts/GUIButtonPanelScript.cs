using UnityEngine;
using System.Collections;

public class GUIButtonPanelScript : MonoBehaviour {

	public float percentLeftOfScreen;
	public float percentBottomOfScreen;
	public float buttonWidth;
	public float buttonHeight;
	public float layoutPanelOffset;
	public int buttonOffset;
	// TODO: this is a string array right now for button names but it should be a gameobject or something so we can make the buttons pretty
	public string[] resourceArray;
	
	void OnGUI () {
	
		// I am creating a GUILayout which will be used for the in game overlay
		
		float buttonXLocation = (percentLeftOfScreen*Screen.width/100) - buttonWidth/2;
		float buttonYLocation = (percentBottomOfScreen*Screen.height/100) - buttonHeight/2;
		
		GUI.Box(new Rect(buttonXLocation - layoutPanelOffset, buttonYLocation - layoutPanelOffset, buttonWidth + layoutPanelOffset*2, buttonHeight + layoutPanelOffset*2), "");
		GUILayout.BeginArea(new Rect(buttonXLocation, buttonYLocation, buttonWidth, buttonHeight) );
		foreach(string button in resourceArray){
			GUILayout.Button(button);
		}
		GUILayout.EndArea();
		/*
		// Make a button. We pass in the GUIStyle defined above as the style to use
		GUI.Button (new Rect (buttonXLocation, buttonYLocation, buttonWidth, buttonHeight), "I am a Custom Button");
		
		GUI.Button(new Rect(Screen.width/2, Screen.height/2, 10,10), "Center"); */
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
}
