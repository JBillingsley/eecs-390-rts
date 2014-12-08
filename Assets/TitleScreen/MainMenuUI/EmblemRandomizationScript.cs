using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EmblemRandomizationScript : MonoBehaviour {

	public Image title;
	public Sprite[] titles;
	public int titleToUse;
	
	void Start(){
		if(title == null){
			title = transform.GetComponent<Image>();
		}
		titleToUse = (int)Random.Range(0, titles.Length);
		title.sprite = titles[titleToUse];
	}
}
