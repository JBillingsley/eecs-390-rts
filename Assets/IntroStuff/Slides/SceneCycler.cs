using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneCycler : MonoBehaviour {

	public float cycleSpeed;
	public Image[] scenes;
	public string sceneToLoad;
	public int count = 0;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < scenes.Length - 1; i++){
			Invoke("cycleScene", cycleSpeed * (i +1));
		}
		Invoke("cycleScene", cycleSpeed * scenes.Length);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if(count == scenes.Length + 1){
			Application.LoadLevel(sceneToLoad);
		}
	}
	
	public void cycleScene(){
		
		count = count + 1;
		if(count >= scenes.Length){
			if(sceneToLoad != null){
				Application.LoadLevel(sceneToLoad);
			}
			return;
		}
		scenes[count-1].enabled = false;
		toggleChildren(count -1, false);
		scenes[count].enabled = true;
		toggleChildren(count, true);
	}
	
	public void toggleChildren(int number, bool enabled){
		foreach(Image image in scenes[number].GetComponentsInChildren<Image>()){
			image.enabled = enabled;
		}
	}
}
