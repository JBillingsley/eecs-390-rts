using UnityEngine;
using System.Collections;

public class EmblemRandomizationScript : MonoBehaviour {

	public Texture[] titles;
	public float heightOffset;
	public float scale;
	public int titleToUse;
	public float ratio;
	
	void Start(){
		titleToUse = (int)Random.Range(0, titles.Length);
		scale = scale / 100;
	}
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width/2 - titles[titleToUse].width/2*scale, Screen.height - Screen.height/heightOffset, titles[titleToUse].width*scale, titles[titleToUse].height*scale), titles[titleToUse]);
	}
}
