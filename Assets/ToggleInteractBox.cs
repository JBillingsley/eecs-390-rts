using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleInteractBox : MonoBehaviour {

	public Canvas interactPane;
	bool activeState = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			activeState = false;
			interactPane.enabled = activeState;
		}
	}
	
	public void toggle(){
	Debug.Log("tongling sutf");
		activeState = !activeState;
		interactPane.enabled = activeState;
	}
		
}
