using UnityEngine;
using System.Collections;

public class ButtonPresser : MonoBehaviour {

	public LayerMask buttons;
	private Button lastButton;

	void Start(){
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

	void OnGUI(){
		RaycastHit2D hit;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		Button b = null;

		if(hit = Physics2D.GetRayIntersection(r,100f,buttons.value)){
			b = (hit.collider.GetComponent(typeof(Button)) as Button);
		}
		if(b != null){
			if(Input.GetMouseButtonDown(0)){
				if(hit = Physics2D.GetRayIntersection(r,100f,buttons.value)){
					b.press();
					return;
				}
			}
			else{
				b.mouseOver();
			}
		}
		if(b != lastButton){
			if(lastButton){
				lastButton.mouseOff();
			}
			lastButton = b;
		}
	}
}
