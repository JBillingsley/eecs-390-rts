using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CityManager : MonoBehaviour {

	public InteractionHandler[] cities;
	private bool[] cityFlags;
	private int count = 0;
	
	public static CityManager _instance;
	// Use this for initialization
	void Start () {
		_instance = this;
		cityFlags = new bool[]{false, false, false};
	}
	
	public static CityManager instance {
		get {
			if(instance == null){
				_instance = GameObject.FindObjectOfType<CityManager>();
			}
			return _instance;
		}
	}
	
	public void updateCityFlag(int city){
		if(cityFlags[city] == false){
			cityFlags[city] = true;
			count += 1;
			checkAllTrue();
		}
	}
	
	public void checkAllTrue(){
		if(count >= cityFlags.Length){
			for(int i = 0; i < cities.Length; i++){
				cities[i].updateText();
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
