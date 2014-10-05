using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (AnimationSet))]
public class AnimationsetDrawer : PropertyDrawer {

	public const int texSize = 100;
	public const int padding = 10;
	private static GUIStyle style = new GUIStyle();
	private static Texture2D cache;

	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight (prop);
	}

	public static float calculateHeight(SerializedProperty prop){
		float h = EditorUtil.row;
		SerializedProperty animations = prop.FindPropertyRelative ("animations");
		if (!prop.FindPropertyRelative ("folded").boolValue){
			int i;
			for (i = 0; i < animations.arraySize; i++) {
				SerializedProperty animation = animations.GetArrayElementAtIndex(i);
				h += AnimationDrawer.calculateHeight(animation);
			}
			h += texSize;
		}
		return h + EditorUtil.padding;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty tilesetName = prop.FindPropertyRelative ("tilesetName");
		SerializedProperty animations = prop.FindPropertyRelative ("animations");
		SerializedProperty preview = prop.FindPropertyRelative ("preview");
		SerializedProperty previewIndex = prop.FindPropertyRelative ("previewIndex");
		SerializedProperty tilesetID = prop.FindPropertyRelative ("tileset");

		SerializedProperty folded = prop.FindPropertyRelative ("folded");
		bool fold = folded.boolValue;

		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorUtil.textField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), name, !fold, "Animation Set Name");

		if (!fold){
			if (EditorUtil.plus(pos.x + pos.width - EditorUtil.buttonSize, pos.y, "New Animation"))
				Animation.construct(animations.GetArrayElementAtIndex(animations.arraySize++));
		}
	
		EditorGUI.indentLevel++;
		if (!fold){
			tilesetID.intValue = EditorGUI.Popup(new Rect (pos.x, pos.y + 2 * EditorUtil.row, pos.width - texSize - padding, EditorUtil.height), "Tileset", tilesetID.intValue, GlobalData.getTilesetNames());

			Tileset tileset = GlobalData.getTileset(tilesetID.intValue);

			preview.intValue = Mathf.Max (-1, Mathf.Min(preview.intValue, animations.arraySize - 1));

			SerializedProperty animation = animations.GetArrayElementAtIndex(preview.intValue);
			SerializedProperty sequence = animation.FindPropertyRelative("tileSequence");
			SerializedProperty frameskip = animation.FindPropertyRelative("frameSkip");
			SerializedProperty lastFrame = prop.FindPropertyRelative("lastFrame");

			float time = Time.realtimeSinceStartup;
			if (time - lastFrame.floatValue < 0)
				lastFrame.floatValue = time;
			if (time - lastFrame.floatValue > frameskip.intValue * Time.fixedDeltaTime){
				previewIndex.intValue = (previewIndex.intValue + 1) % (sequence.arraySize);
				lastFrame.floatValue = time;
			}

			GUIContent content = getPreview(tileset, previewIndex.intValue, sequence);
			if (content == null){
				float aspect = tileset.aspect();
				if (aspect < 1)
					GUI.Box(new Rect (pos.x + pos.width - texSize*aspect, pos.y + EditorUtil.row, texSize*aspect, texSize), GUIContent.none, style);
				else
					GUI.Box(new Rect (pos.x + pos.width - texSize, pos.y + EditorUtil.row + (1 - 1/aspect) *  texSize / 2, texSize, texSize/aspect), GUIContent.none, style);
			}
			else
				GUI.Box(new Rect (pos.x + pos.width - texSize, pos.y + EditorUtil.row, texSize, texSize), content);
		
			/*Draw Animations*/
			float ay = EditorUtil.row + EditorUtil.texSize + EditorUtil.padding;
			float[] h = new float[animations.arraySize];
			for (int i = 0; i < animations.arraySize; i++) {
				SerializedProperty anim = animations.GetArrayElementAtIndex(i);
				float animationHeight = AnimationDrawer.calculateHeight(anim);
				//Draw Animation
				EditorGUI.PropertyField (new Rect (pos.x, pos.y + ay, pos.width, animationHeight), anim, GUIContent.none);
				//Draw Preview Button
				if(GUI.Button (new Rect (pos.x + pos.width - EditorUtil.buttonSize - AnimationDrawer.previewWidth, pos.y + ay, AnimationDrawer.previewWidth, EditorUtil.height), new GUIContent("Preview", "Preview Animation")))
					preview.intValue = i;
				//Draw Delete Button
				if(EditorUtil.ex(pos.x + pos.width - EditorUtil.buttonSize, pos.y + ay, "Delete"))
					animations.DeleteArrayElementAtIndex(i);

				h[i] = ay;
				ay += animationHeight;
			}	
			EditorUtil.foldLines(pos.x + EditorGUI.indentLevel * 12, pos.y, h);
		}
		EditorGUI.indentLevel--;

		prop.serializedObject.ApplyModifiedProperties ();
	}

	private GUIContent getPreview(Tileset tileset, int index, SerializedProperty sequence){
		if (tileset == null)
			return new GUIContent("No valid Tileset found");
		if (tileset.texture == null)
			return new GUIContent("Tileset has no valid texture");
		if (index == -1)
			return new GUIContent(tileset.texture);
		index = (index+1)%sequence.arraySize;
		Texture2D tex = tileset.getSubtexture(cache, sequence.GetArrayElementAtIndex(index).intValue % (tileset.width * tileset.height));
		if (style.normal.background != tex)
			style.normal.background = tex;
		return tex == null ? new GUIContent("Texture cannot be read") : null;
	}
}