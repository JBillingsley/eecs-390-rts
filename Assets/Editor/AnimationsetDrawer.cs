using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (AnimationSet))]
public class AnimationsetDrawer : PropertyDrawer {
	
	public const int buttonWidth = 20;
	public const int texSize = 100;
	public const int padding = 10;
	public static float dy = 16;
	private GUIStyle style = new GUIStyle();

	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		dy = base.GetPropertyHeight (prop, label);
		float h = dy;
		SerializedProperty animations = prop.FindPropertyRelative ("animations");
		if (!prop.FindPropertyRelative ("folded").boolValue){
			for (int i = 0; i < animations.arraySize; i++) {
				SerializedProperty animation = animations.GetArrayElementAtIndex(i);
				h += AnimationDrawer.calculateHeight(animation);
			}
			h += texSize;
		}
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty tilesetName = prop.FindPropertyRelative ("tilesetName");
		SerializedProperty animations = prop.FindPropertyRelative ("animations");
		SerializedProperty folded = prop.FindPropertyRelative ("folded");
		SerializedProperty preview = prop.FindPropertyRelative ("preview");
		SerializedProperty previewIndex = prop.FindPropertyRelative ("previewIndex");

		dy = base.GetPropertyHeight(prop, label);

		if (!folded.boolValue){
			if (GUI.Button (new Rect (pos.x + EditorGUI.indentLevel * 12, pos.y, buttonWidth, dy), new GUIContent("", "Collapse"), (GUIStyle)"OL Minus"))
				folded.boolValue = true;
			EditorGUI.PropertyField (new Rect (pos.x + buttonWidth, pos.y, pos.width - 2 * buttonWidth, dy), name, new GUIContent ("", "Animation Set Name"));
			//New Animation Button
			if (GUI.Button (new Rect (pos.x + pos.width - buttonWidth, pos.y, buttonWidth, dy), new GUIContent("", "New Animation"), (GUIStyle)"OL Plus"))
				Animation.construct(animations.GetArrayElementAtIndex(animations.arraySize++));
		}
		else{
			if (GUI.Button (new Rect (pos.x + EditorGUI.indentLevel * 12, pos.y, buttonWidth, dy), new GUIContent("", "Expand"), (GUIStyle)"OL Plus"))
				folded.boolValue = false;
			EditorGUI.LabelField (new Rect (pos.x + buttonWidth, pos.y, pos.width - buttonWidth, dy), new GUIContent (name.stringValue, "Animation Set Name"));
		}
		EditorGUI.indentLevel++;
		if (!folded.boolValue){
			EditorGUI.PropertyField (new Rect (pos.x, pos.y + dy, pos.width - texSize - padding, dy), tilesetName, new GUIContent ("Tileset", "Tileset Name"));

			Tileset tile = (Tileset)Global.getTileset(tilesetName.stringValue);
			if (Event.current.type == EventType.Repaint) { 
				Rect pre = new Rect (pos.x + pos.width - texSize, pos.y + dy, texSize, texSize);
				if (tile == null)
					GUI.Box(pre, "No valid Tileset found");
				else if (tile.texture == null)
					GUI.Box(pre, "Tileset has no valid texture");
				else{
					int i = preview.intValue;
					if (i < 0 || i >= animations.arraySize)
						GUI.Box(pre, tile.texture);
					else {
						SerializedProperty animation = animations.GetArrayElementAtIndex(i);
						SerializedProperty sequence = animation.FindPropertyRelative("tileSequence");
						i = (i+1)%sequence.arraySize;
						Texture2D tex = tile.getSubtexture(sequence.GetArrayElementAtIndex(i).intValue % (tile.width * tile.height));
						GUIContent content = tex == null ? new GUIContent("Texture cannot be read") : new GUIContent(tex);
						
						Debug.Log (tex.width + " " + tex.height);
						//GUI.Box(pre, content);
						Graphics.DrawTexture(pre, tex);
						preview.intValue = i;
					}
				}

			}
			float cy = dy + texSize;
			for (int i = 0; i < animations.arraySize; i++) {
				SerializedProperty animation = animations.GetArrayElementAtIndex(i);
				float animationHeight = AnimationDrawer.calculateHeight(animation);
				EditorGUI.PropertyField (new Rect (pos.x, pos.y + cy, pos.width, animationHeight), animation, GUIContent.none);
				if(GUI.Button (new Rect (pos.x + pos.width - buttonWidth - AnimationDrawer.previewWidth, pos.y + cy, AnimationDrawer.previewWidth, dy), new GUIContent("Preview", "Preview Animation"))){
					preview.intValue = i;
				}
				if(GUI.Button (new Rect (pos.x + pos.width - buttonWidth, pos.y + cy, buttonWidth, dy), new GUIContent("", "Delete"), (GUIStyle)"WinBtnCloseWin")){
					animations.DeleteArrayElementAtIndex(i);
				}
				cy += animationHeight;
			}
		}
		EditorGUI.indentLevel--;

		prop.serializedObject.ApplyModifiedProperties ();
	}


}