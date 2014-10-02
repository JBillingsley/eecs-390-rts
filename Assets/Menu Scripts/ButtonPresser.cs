using UnityEngine;
using System.Collections;

public class ButtonPresser : MonoBehaviour {

	public LayerMask buttons;

	void Start(){
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

	void OnGUI(){
		RaycastHit2D hit;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Input.GetMouseButtonDown(0)){
			if(hit = Physics2D.GetRayIntersection(r,100f,buttons.value)){
				(hit.collider.GetComponent(typeof(Button)) as Button).press();
				return;
			}
		}
	}
}
