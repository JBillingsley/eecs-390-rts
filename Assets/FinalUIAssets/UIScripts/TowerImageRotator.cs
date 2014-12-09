using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerImageRotator : MonoBehaviour {
	
	public Sprite[] sprites;
	public Image image;
	
	public void rotateSprites(int count){
		image.sprite = sprites[count];
	}
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
