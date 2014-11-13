using UnityEngine;
using System.Collections;

public class PanelButton : MonoBehaviour {

	public string content;
	public Camera guiCam;
	public GameObject panel;
	public Vector3 scale;
	public float x;
	public float y;
	public Texture texture;
	
	void OnGUI(){
		
		GUI.depth = -1;
		float camHalfHeight = guiCam.orthographicSize;
		float camHalfWidth = guiCam.aspect * camHalfHeight; 
		Vector3 panelSize = panel.renderer.bounds.size;
		
		Vector3 toolTipActualPosition = Camera.main.WorldToScreenPoint(transform.position);
		worldToScreenPositions();
		
		GUI.Button(new Rect(toolTipActualPosition.x, Screen.height - toolTipActualPosition.y, ((x * scale.x) * Screen.width)/100, ((y * scale.y)* Screen.height)/100), content);
		//GUI.DrawTexture(new Rect(0, 0,((x * scale.x) * Screen.width)/100, ((y * scale.y)* Screen.height)/100), texture);
	
	}
	
	private void worldToScreenPositions(){
		float currentObjectWidth = panel.renderer.bounds.size.x;
		float currentObjectHeight = panel.renderer.bounds.size.y;
		
		Vector3 scale = transform.localScale;
		
		scale.x = x * scale.x/currentObjectWidth;
		scale.y = y * scale.y/currentObjectHeight;
		
		this.scale = scale;
	}
}
