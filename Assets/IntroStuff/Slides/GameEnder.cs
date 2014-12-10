using UnityEngine;
using System.Collections;

public class GameEnder : MonoBehaviour {

	public string sceneToLoad;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)){
			Application.LoadLevel(sceneToLoad);
		}
	}
}
