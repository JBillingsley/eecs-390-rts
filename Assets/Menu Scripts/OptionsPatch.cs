using UnityEngine;
using System.Collections;

public class OptionsPatch : MonoBehaviour {

	public GameObject options;
	
	public void openOptions(){
		if(!options.activeSelf){
			options.SetActive(true);
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
