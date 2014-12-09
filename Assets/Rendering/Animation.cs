using UnityEngine;
using System.Collections;

[System.Serializable]
public class Animation {
	public string name = "Animation Name";
	[FixedAttribute()]
	public int[] tileSequence = new int[1];
	[Range(1, 60)]
	public int frameSkip = 1;
	[SerializeField]
	private bool folded;




}
