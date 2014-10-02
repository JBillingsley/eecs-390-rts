using UnityEngine;
using System.Collections;

public class LevelButton : Button {

	public string levelName;

	public override void press () {
		Application.LoadLevel(levelName);
	}
}
