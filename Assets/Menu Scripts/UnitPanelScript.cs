using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitPanelScript : MonoBehaviour {

	public Camera guiCam;
	public GameObject panel;
	public Vector3 scale;
	public float x;
	public float y;
	private ArrayList selectedUnits;
	public Texture texture;
	
	void Start(){
		selectedUnits = new ArrayList();
	}
	
	public void addSelectedUnit(GameObject unit){
		selectedUnits.Add(unit);
	}
	
	public void removeSelectedUnit(GameObject unit){
		selectedUnits.Remove(selectedUnits.IndexOf(unit));
	}
	
	private void updateUnitList(){
		foreach(GameObject unit in selectedUnits){
			GUI.Button(new Rect(0, 0,((x * scale.x) * Screen.width)/100, ((y * scale.y)* Screen.height)/100*selectedUnits.Count), "sup");
		}
	}
	
	void OnGUI(){
		updateUnitList();
		
		float camHalfHeight = guiCam.orthographicSize;
		float camHalfWidth = guiCam.aspect * camHalfHeight; 
		Vector3 panelSize = panel.renderer.bounds.size;
		
		Vector3 toolTipActualPosition = Camera.main.WorldToScreenPoint(transform.position);
		worldToScreenPositions();
		
		GUI.BeginGroup(new Rect(toolTipActualPosition.x, Screen.height - toolTipActualPosition.y, ((x * scale.x) * Screen.width)/100, ((y * scale.y)* Screen.height)/100));
		GUI.DrawTexture(new Rect(0, 0,((x * scale.x) * Screen.width)/100, ((y * scale.y)* Screen.height)/100), texture);
		updateUnitList();
		GUI.EndGroup();
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
