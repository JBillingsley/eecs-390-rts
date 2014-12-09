using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (Animation))]
public class AnimationDrawer : PropertyDrawer {

	public const int row = 10;
	public const int previewWidth = 60;
	private GUIStyle style = new GUIStyle();
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight (prop);
	}

	public static void construct(SerializedProperty prop){
		prop.FindPropertyRelative("name").stringValue = "Animation Name";
		SerializedProperty sequence = prop.FindPropertyRelative ("tileSequence");
		sequence.arraySize = 1;
		sequence.GetArrayElementAtIndex (0).intValue = 0;
		prop.FindPropertyRelative ("frameSkip").intValue = 1;
		prop.serializedObject.ApplyModifiedProperties();
	}
	
	public static float calculateHeight(SerializedProperty prop){
		SerializedProperty tileSequence = prop.FindPropertyRelative ("tileSequence");
		if (prop.FindPropertyRelative ("folded").boolValue)
			return EditorUtil.row;
		return (((tileSequence.arraySize - 1) / row) + 4) * EditorUtil.row;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty frameSkip = prop.FindPropertyRelative ("frameSkip");
		SerializedProperty tileSequence = prop.FindPropertyRelative ("tileSequence");
		SerializedProperty folded = prop.FindPropertyRelative ("folded");

		if (name == null || frameSkip == null || tileSequence == null)
			return;

		bool fold = folded.boolValue;

		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorUtil.textField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize - previewWidth, EditorUtil.height), name, !fold, "Animation Name");
	
		EditorGUI.indentLevel++;
		if (!folded.boolValue){
			//Draw Frame Rate
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + EditorUtil.row, pos.width, EditorUtil.height), frameSkip, new GUIContent("Framerate"));
			//Draw Frame Sequence
			EditorGUI.LabelField (new Rect (pos.x, pos.y + 2 * EditorUtil.row, pos.width, EditorUtil.height), new GUIContent("Frame Sequence"));
			
			if (GUI.Button (new Rect (pos.x + pos.width - EditorUtil.buttonSize, pos.y + 2 * EditorUtil.row, EditorUtil.buttonSize, EditorUtil.height), new GUIContent ("+"), EditorStyles.miniButtonRight)) {
					tileSequence.arraySize++;
			}
			if (GUI.Button (new Rect (pos.x + pos.width - 2 * EditorUtil.row, pos.y + 2 * EditorUtil.row, EditorUtil.buttonSize, EditorUtil.height), new GUIContent ("-"), EditorStyles.miniButtonLeft)) {
				if (tileSequence.arraySize > 1)
					tileSequence.arraySize--;
			}

			float w = pos.width;
			
			float width = w / row;
			float adjSize = 45;
			float adj = adjSize / row;

			for (int i = 0; i < tileSequence.arraySize; i++){
				int xx = (i % row);
				int yy = (i / row);
				EditorGUI.PropertyField (new Rect (pos.x + xx * (width - adj), pos.y + (3 + yy) * EditorUtil.row, width - adj + adjSize, EditorUtil.height), tileSequence.GetArrayElementAtIndex(i), GUIContent.none);
			}
		}
		EditorGUI.indentLevel--;

		prop.serializedObject.ApplyModifiedProperties ();
	}
	
	
}