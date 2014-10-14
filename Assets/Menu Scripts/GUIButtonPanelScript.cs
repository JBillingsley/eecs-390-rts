using UnityEngine;
using System.Collections;

public class GUIButtonPanelScript : MonoBehaviour {

	public float percentLeftOfScreen;
	public float percentBottomOfScreen;
	public float buttonWidth;
	public float buttonHeight;
	public float layoutPanelOffset;
	public Vector3 offsetScale;
	public int buttonOffset;
	public Vector3 buttonScale;
	public float buttonXLocation;
	public float buttonYLocation;
	// TODO: this is a string array right now for button names but it should be a gameobject or something so we can make the buttons pretty
	public string[] resourceArray;
	
	void OnGUI () {
		worldToScreenPositions();
	
		// I am creating a GUILayout which will be used for the in game overlay
		buttonXLocation = (percentLeftOfScreen*Screen.width/100) - buttonWidth/2;
		buttonYLocation = (percentBottomOfScreen*Screen.height/100) - buttonHeight/2;
		
		GUI.Box(new Rect((buttonXLocation - layoutPanelOffset), (buttonYLocation - layoutPanelOffset), (buttonWidth + layoutPanelOffset*2)*offsetScale.x, (buttonHeight + layoutPanelOffset*2)*offsetScale.y), "");
		GUILayout.BeginArea(new Rect(buttonXLocation, buttonYLocation, buttonWidth*buttonScale.x, buttonHeight*buttonScale.y) );
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
	
	private void worldToScreenPositions(){
		float currentObjectWidth = gameObject.renderer.bounds.size.x;
		float currentObjectHeight = gameObject.renderer.bounds.size.y;
		
		Vector3 scale = transform.localScale;
		
		scale.x = buttonWidth * scale.x/currentObjectWidth;
		scale.y = buttonHeight * scale.y/currentObjectHeight;
		
		this.buttonScale = scale;
		
		Vector3 offsetScale = transform.localScale;
		offsetScale.x = (buttonWidth + layoutPanelOffset*2) * scale.x / currentObjectWidth;
		offsetScale.y = (buttonHeight + layoutPanelOffset*2)*scale.y / currentObjectHeight;
		this.offsetScale = offsetScale;
		
		//transform.localScale = scale;
	}
	
	
}
