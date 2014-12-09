using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerHotBarScript : MonoBehaviour {

	public CraftingReqs[] reqs;
	public CraftingAmtTextUpdateScript text;
	public TowerHotBarScript otherButton;
	public int count = 0;
	private Tower tower;
	
	// Use this for initialization
	void Start () {
		if(tower == null){
			tower = GameObject.FindObjectOfType<Tower>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		text.updateText(reqs[count]);
	}
	
	public void addLayer(){
		if(reqs[count].updateResources() > 0){
			reqs[count].makeDude();
			tower.increaseHeight();
		}
	}
	
	public void upgradeTower(){
		if(reqs[count].updateResources() > 0 && count < reqs.Length - 1 && tower.type < Tower.TowerType.STONE){
			reqs[count].makeDude();
			count += 1;
			otherButton.count += 1;
			transform.GetComponent<TowerImageRotator>().rotateSprites(count);
			tower.type = tower.type+1;
			tower.increaseHeight();
			tower.decreaseHeight();
			
		}
	}
}
