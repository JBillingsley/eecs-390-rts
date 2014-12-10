using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour {
	
	public int cityNumber;
	public string winText;
	public string[] words;
	int count = -1;
	public Text text;
	// Use this for initialization
	void Start () {
		//text.text = words[count];
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void cycleText(){
		if(count < words.Length-1){
			count += 1;
			text.text = words[count];
			CityManager.instance.updateCityFlag(cityNumber);
		}
	}
	
	public void updateText(){
	Debug.Log("we win");
		text.text = winText;
	}
}
