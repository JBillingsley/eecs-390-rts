using UnityEngine;
using System.Collections;

public class TitleRenderingScript : MonoBehaviour {

	public Texture titleTexture;
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), titleTexture);
	}
}
