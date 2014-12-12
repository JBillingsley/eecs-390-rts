using UnityEngine;
using System.Collections;

public class AudioSurpresser : MonoBehaviour {

	private static bool busy;

	private AudioSource source;
	private AudioSource source2;
	private Player2 player;

	bool active = false;
	bool ramp = false;
	float activeTime = 0;

	public float startDistance = 20;	

	void Start(){
		source = GameObject.FindObjectOfType<Camera2D>().audio;
		source2 = this.audio;
		player = GameObject.FindObjectOfType<Player2>();
	}

	void FixedUpdate(){
		Vector2 dif = player.transform.position - this.transform.position;
		Debug.Log (dif.magnitude);
		if(dif.magnitude < startDistance && ! busy){
			if(!active){
				activeTime = 0;
			}
			active = true;
			activeTime += Time.fixedDeltaTime;
			source.volume = Mathf.Lerp(1,0,activeTime);
		}
		else if (active && dif.magnitude > startDistance + 1){
			active = false;
			ramp = true;
			activeTime = 0;
		}
		else if(ramp){
			busy = true;
			activeTime += Time.fixedDeltaTime;
			source.volume = Mathf.Lerp(0,1,activeTime);
			if(activeTime > 5){
				busy = false;
				ramp = false;
				source.volume = 1;
				activeTime = 0;
			}
		}
		//source.volume = Mathf.Clamp(dif.magnitude / startDistance,0,1);
	}
}
