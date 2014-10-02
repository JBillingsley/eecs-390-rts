using UnityEngine;
using System.Collections;

public class LevelButton : Button {

	public string levelName;

	public override void press () {
		Application.LoadLevel(levelName);
	}

	public override void mouseOver(){
		this.renderer.material.color = Color.red;
	}

	public override void mouseOff(){
		this.renderer.material.color = Color.white;
	}

}
