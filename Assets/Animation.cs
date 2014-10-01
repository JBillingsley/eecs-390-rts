using UnityEngine;
using UnityEditor;
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

	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative("name").stringValue = "Animation Name";                
		prop.FindPropertyRelative ("tileSequence").arraySize = 1;
		prop.FindPropertyRelative ("frameSkip").intValue = 1;
		prop.serializedObject.ApplyModifiedProperties();
	}


}
