using UnityEngine;
using System.Collections;

public abstract class Quest{
	//The help quest to tell the player what to do
	protected string questText;

	//The text to be displayed
	protected string victoryText;

	protected abstract void startQuest();

	protected abstract bool checkCompletion();

	protected abstract void completeQuest();

	protected abstract void trackQuest();

}
