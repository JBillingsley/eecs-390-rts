using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CraftingAmtTextUpdateScript : MonoBehaviour {

	public GameObject craftingButton;
	public Text thisThing;
	
	
	// Update is called once per frame
	void Update () {
		thisThing.text = "" + craftingButton.GetComponent<CraftingReqs>().updateResources();
	}
}
