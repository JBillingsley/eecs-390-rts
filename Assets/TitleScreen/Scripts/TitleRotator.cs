using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleRotator : MonoBehaviour {

	public GameObject[] options;
	int count = 0;
	// Use this for initialization
	void Start () {
		options[count].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void rotate(){
		options[count].SetActive(false);
		count += 1;
		if(count == options.Length){
			count = 0;
		}
		options[count].SetActive(true);
	}
}
