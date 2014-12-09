using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CraftingAmtTextUpdateScript : MonoBehaviour {

	public GameObject craftingButton;
	public Text thisThing;
	
	public void updateText(CraftingReqs req){
		thisThing.text = "" + req.updateResources();
	}
	
	// Update is called once per frame
	void Update () {
		if(craftingButton != null){
			thisThing.text = "" + craftingButton.GetComponent<CraftingReqs>().updateResources();
		}
	}
}
