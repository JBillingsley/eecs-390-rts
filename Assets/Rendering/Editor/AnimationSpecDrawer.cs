using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof (AnimationSpec))]
public class AnimationSpecDrawer : PropertyDrawer {
	
	private static Texture2D cache;
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight (prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		return 2 * EditorUtil.row;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		SerializedProperty animationsetID = prop.FindPropertyRelative ("animationsetID");
		SerializedProperty animationID = prop.FindPropertyRelative ("animationID");

		animationsetID.intValue = EditorGUI.Popup(new Rect (pos.x, pos.y, pos.width, EditorUtil.height), "Animation Set", animationsetID.intValue, AnimationSetList.getAnimationSetNames());
		animationID.intValue = EditorGUI.Popup(new Rect (pos.x, pos.y + EditorUtil.row, pos.width, EditorUtil.height), "Animation", animationID.intValue, AnimationSetList.getAnimationNames(animationsetID.intValue));

		prop.serializedObject.ApplyModifiedProperties ();
	}

}
