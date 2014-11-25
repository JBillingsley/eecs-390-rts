using UnityEngine;
using System.Collections;

public class CityInteractScript : MonoBehaviour {
	
	public GameObject textBox;
	public bool textActive = false;
	public bool playerInRange = false;
	
	void OnMouseDown(){
			textActive = true;
			textBox.GetComponent<TextPopupScript>().enabled = textActive;
	}
	
	// Use this for initialization
	void Start () {
		textBox.GetComponent<TextPopupScript>().enabled = textActive;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			textActive = false;
			textBox.GetComponent<TextPopupScript>().enabled = textActive;
		}
	}
}
