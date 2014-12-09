using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer (typeof (TextureAtlasList))]
public class TextureAtlasListDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight (prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		float h = EditorUtil.row;
		if (!prop.FindPropertyRelative ("textureAtlasFolded").boolValue){
			SerializedProperty textureAtlases = prop.FindPropertyRelative ("textureAtlases");
			for (int i = 0; i < textureAtlases.arraySize; i++) {
				SerializedProperty textureAtlas = textureAtlases.GetArrayElementAtIndex(i);
				h += TextureAtlasDrawer.calculateHeight(textureAtlas);
			}
		}
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		renderTextureAtlass (pos, prop);
	}

	private float renderTextureAtlass(Rect pos, SerializedProperty prop){
		SerializedProperty folded = prop.FindPropertyRelative ("textureAtlasFolded");
		bool fold = folded.boolValue;
		
		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorGUI.LabelField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), new GUIContent("Texture Atlases"));
		
		SerializedProperty textureAtlases = prop.FindPropertyRelative ("textureAtlases");
		if (EditorUtil.plus(pos.x + pos.width - EditorUtil.buttonSize, pos.y, "New Texture Atlas"))
			TextureAtlasDrawer.construct(textureAtlases.GetArrayElementAtIndex(textureAtlases.arraySize++));
		float ay = EditorUtil.row;
		if (textureAtlases == null)
			return ay;
		if (!fold){
			EditorGUI.indentLevel++;
			float[] h = new float[textureAtlases.arraySize];
			for (int i = 0; i < textureAtlases.arraySize; i++) {
				SerializedProperty textureAtlas = textureAtlases.GetArrayElementAtIndex(i);
				float textureAtlasHeight = TextureAtlasDrawer.calculateHeight(textureAtlas);
				
				//Draw TextureAtlas
				EditorGUI.PropertyField (new Rect (pos.x, pos.y + ay, pos.width, textureAtlasHeight), textureAtlas, GUIContent.none);
				
				if (textureAtlas.FindPropertyRelative("folded").boolValue){
					if (EditorUtil.ex(pos.x + pos.width - EditorUtil.buttonSize, pos.y + ay, "Delete Texture Atlas"))
						textureAtlases.DeleteArrayElementAtIndex(i);
				}
				
				h[i] = ay;
				ay += textureAtlasHeight;
			}
			EditorUtil.foldLines(pos.x + EditorGUI.indentLevel * 12, pos.y, h);
			EditorGUI.indentLevel--;
		}
		return ay;
	}
}