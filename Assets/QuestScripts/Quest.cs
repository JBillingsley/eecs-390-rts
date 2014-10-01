using UnityEngine;
using System.Collections;

public abstract class Quest{
	//The help quest to tell the player what to do
	string questText;

	//The text to be displayed
	string victoryText;

	protected abstract void startQuest();

	protected abstract void checkCompletion();

	protected abstract void completeQuest();

	protected abstract void trackQuest();

}
