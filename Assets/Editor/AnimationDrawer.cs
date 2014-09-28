using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (Animation))]
public class AnimationDrawer : PropertyDrawer {

	public const int row = 6;
	public const int buttonWidth = 20;
	public const int previewWidth = 60;
	public static float dy = 16;
	private GUIStyle style = new GUIStyle();
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		dy = base.GetPropertyHeight (prop, label);
		return calculateHeight (prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		SerializedProperty tileSequence = prop.FindPropertyRelative ("tileSequence");
		if (prop.FindPropertyRelative ("folded").boolValue)
			return dy;
		return (((tileSequence.arraySize - 1) / row) + 4) * dy;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty frameSkip = prop.FindPropertyRelative ("frameSkip");
		SerializedProperty tileSequence = prop.FindPropertyRelative ("tileSequence");
		SerializedProperty folded = prop.FindPropertyRelative ("folded");

		if (name == null || frameSkip == null || tileSequence == null)
			return;
		
		dy = base.GetPropertyHeight(prop, label);

		if (!folded.boolValue){
			if (GUI.Button (new Rect (pos.x + EditorGUI.indentLevel * 12, pos.y, buttonWidth, dy), new GUIContent("", "Collapse"), (GUIStyle)"OL Minus"))
				folded.boolValue = true;
			EditorGUI.PropertyField (new Rect (pos.x + buttonWidth, pos.y, pos.width - 2 * buttonWidth - previewWidth, dy), name, new GUIContent ("", "Animation Name"));
		}
		else{
			if (GUI.Button (new Rect (pos.x + EditorGUI.indentLevel * 12, pos.y, buttonWidth, dy), new GUIContent("", "Expand"), (GUIStyle)"OL Plus"))
				folded.boolValue = false;
			EditorGUI.LabelField (new Rect (pos.x + buttonWidth, pos.y, pos.width - buttonWidth - previewWidth, dy), new GUIContent (name.stringValue, "Animation Name"));
		}
		EditorGUI.indentLevel++;
		if (!folded.boolValue){
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + dy, pos.width, dy), frameSkip, new GUIContent("Frame Rate"));
			EditorGUI.LabelField (new Rect (pos.x, pos.y + 2 * dy, pos.width, dy), new GUIContent("Frame Sequence"));
			
			if (GUI.Button (new Rect (pos.x + pos.width - buttonWidth, pos.y + 2 * dy, buttonWidth, dy), new GUIContent ("+"), EditorStyles.miniButtonRight)) {
					tileSequence.arraySize++;
			}
			if (GUI.Button (new Rect (pos.x + pos.width - 2 * buttonWidth, pos.y + 2 * dy, buttonWidth, dy), new GUIContent ("-"), EditorStyles.miniButtonLeft)) {
				if (tileSequence.arraySize > 1)
					tileSequence.arraySize--;
			}
			float w = pos.width;
			
			float width = w / row;
			float adjSize = 45;
			float adj = adjSize / row;
			for (int i = 0; i < tileSequence.arraySize; i++){
				EditorGUI.PropertyField (new Rect (pos.x + (i % row) * (width - adj), pos.y + (3 + i / row) * dy, width - adj + adjSize, dy), tileSequence.GetArrayElementAtIndex(i), GUIContent.none);
			}
		}
		EditorGUI.indentLevel--;








		prop.serializedObject.ApplyModifiedProperties ();
	}
	
	
}