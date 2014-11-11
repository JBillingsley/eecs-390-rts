using UnityEngine;
using System.Collections;

public class InGameMenuButtons : MonoBehaviour {


	public string content;
	public float overlayHeight;
	public float overlayWidth;
	public Vector3 toolTipActualPosition;
	public bool buttonMousedOver = false;
	public Vector3 scale;
	
	void OnMouseOver(){
		buttonMousedOver = true;
	}
	
	void OnMouseExit(){
		buttonMousedOver = false;
	}
	
	void OnGUI(){
		if(buttonMousedOver){
			worldToScreenPositions();
			GUI.TextArea(new Rect(toolTipActualPosition.x , Screen.height - (toolTipActualPosition.y)+scale.y, ((overlayWidth * scale.x) * Screen.width)/100, ((overlayHeight * scale.y)* Screen.height)/100), content);
		}
		
	}
	
	// Use this for initialization
	void Start () {
		worldToScreenPositions();
		
	}
	// Update is called once per frame
	void Update () {
		toolTipActualPosition = Camera.main.WorldToScreenPoint(transform.position);
	}
	
	private void worldToScreenPositions(){
		float currentObjectWidth = gameObject.renderer.bounds.size.x;
		float currentObjectHeight = gameObject.renderer.bounds.size.y;
		
		Vector3 scale = transform.localScale;
		
		scale.x = overlayWidth * scale.x/currentObjectWidth;
		scale.y = overlayHeight * scale.y/currentObjectHeight;
		
		this.scale = scale;
	}
}
