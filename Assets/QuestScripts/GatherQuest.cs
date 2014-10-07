using UnityEngine;
using System.Collections;

//A quest requireing you to gather a certain amount of a reasource.
public class GatherQuest : Quest {

	//Resource type

	//Amount needed
	public int amountRequired;

	//Basic constructor for the quest
	public GatherQuest(string message, string endMessage){
		questText = message;
		victoryText = endMessage;
	}

	protected override void startQuest(){

	}
	protected override bool checkCompletion(){
		//If you have the required amount of the resource
			//Return true
		return false;
	}
	protected override void completeQuest(){

	}
	protected override void trackQuest(){

	}
}
