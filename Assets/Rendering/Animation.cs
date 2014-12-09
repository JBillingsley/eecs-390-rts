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
		SerializedProperty sequence = prop.FindPropertyRelative ("tileSequence");
		sequence.arraySize = 1;
		sequence.GetArrayElementAtIndex (0).intValue = 0;
		prop.FindPropertyRelative ("frameSkip").intValue = 1;
		prop.serializedObject.ApplyModifiedProperties();
	}

}
