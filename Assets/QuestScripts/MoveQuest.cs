using UnityEngine;
using System.Collections;

public class MoveQuest : Quest {
	
	//The place to move to
	public IVector2 destination;

	private Character trackedCharacter;

	//Basic constructor for the quest
	public MoveQuest(string message, string endMessage){
		questText = message;
		victoryText = endMessage;
	}
	
	protected override void startQuest(){
		
	}
	protected override bool checkCompletion(){
		if((trackedCharacter.position - (Vector2)destination).magnitude < 1){
			return true;
		}
		return false;
	}
	protected override void completeQuest(){
		
	}
	protected override void trackQuest(){
		
	}
}
