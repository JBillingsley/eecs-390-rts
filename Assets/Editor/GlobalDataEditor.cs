using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer (typeof (GlobalData))]
public class GlobalDataDrawer : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight (prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		float h = 2 * EditorUtil.row;
		if (!prop.FindPropertyRelative ("tilesetFolded").boolValue){
			SerializedProperty tilesets = prop.FindPropertyRelative ("tilesets");
			for (int i = 0; i < tilesets.arraySize; i++) {
				SerializedProperty tileset = tilesets.GetArrayElementAtIndex(i);
				h += TilesetDrawer.calculateHeight(tileset);
			}
		}
		if (!prop.FindPropertyRelative ("animationsetFolded").boolValue){
			SerializedProperty animationsets = prop.FindPropertyRelative ("animationsets");
			for (int i = 0; i < animationsets.arraySize; i++) {
				SerializedProperty animationset = animationsets.GetArrayElementAtIndex(i);
				h += AnimationsetDrawer.calculateHeight(animationset);
			}
		}
		return h;
	}
	
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		GlobalData data = GlobalData.inst;
			
		float h;
		h = renderTilesets (pos, prop);
		pos.y += h; pos.height += h;
		h = renderAnimationsets (pos, prop);
		pos.y += h; pos.height += h;

		
	}

	private float renderTilesets(Rect pos, SerializedProperty prop){
		SerializedProperty folded = prop.FindPropertyRelative ("tilesetFolded");
		bool fold = folded.boolValue;

		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorGUI.LabelField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), new GUIContent("Tilesets"));

		SerializedProperty tilesets = prop.FindPropertyRelative ("tilesets");
		if (EditorUtil.plus(pos.x + pos.width - EditorUtil.buttonSize, pos.y, "New Tileset"))
			Tileset.construct(tilesets.GetArrayElementAtIndex(tilesets.arraySize++));
		float ay = EditorUtil.row;
		if (tilesets == null)
			return ay;
		if (!fold){
			EditorGUI.indentLevel++;
			float[] h = new float[tilesets.arraySize];
			for (int i = 0; i < tilesets.arraySize; i++) {
				SerializedProperty tileset = tilesets.GetArrayElementAtIndex(i);
				float tilesetHeight = TilesetDrawer.calculateHeight(tileset);
				
				//Draw Tileset
				EditorGUI.PropertyField (new Rect (pos.x, pos.y + ay, pos.width, tilesetHeight), tileset, GUIContent.none);

				if (tileset.FindPropertyRelative("folded").boolValue){
					if (EditorUtil.ex(pos.x + pos.width - EditorUtil.buttonSize, pos.y + ay, "Delete Tileset"))
						tilesets.DeleteArrayElementAtIndex(i);
				}
				
				h[i] = ay;
				ay += tilesetHeight;
			}
			EditorUtil.foldLines(pos.x + EditorGUI.indentLevel * 12, pos.y, h);
			EditorGUI.indentLevel--;
		}
		return ay;
	}
	
	private float renderAnimationsets(Rect pos, SerializedProperty prop){
		SerializedProperty folded = prop.FindPropertyRelative ("animationsetFolded");
		bool fold = folded.boolValue;
		EditorUtil.folder (pos.x + EditorGUI.indentLevel * 12, pos.y, folded);
		EditorGUI.LabelField (new Rect (pos.x + EditorUtil.buttonSize, pos.y, pos.width - 2 * EditorUtil.buttonSize, EditorUtil.height), new GUIContent("Animation Sets"));

		SerializedProperty animationsets = prop.FindPropertyRelative ("animationsets");
		if (EditorUtil.plus(pos.x + pos.width - EditorUtil.buttonSize, pos.y, "New Animation Set"))
			AnimationSet.construct(animationsets.GetArrayElementAtIndex(animationsets.arraySize++));

		float ay = EditorUtil.row;
		if (animationsets == null)
			return ay;
		if (!fold){
			EditorGUI.indentLevel++;
			float[] h = new float[animationsets.arraySize];
			for (int i = 0; i < animationsets.arraySize; i++) {
				SerializedProperty animationset = animationsets.GetArrayElementAtIndex(i);
				float animationsetHeight = AnimationsetDrawer.calculateHeight(animationset);
				
				//Draw Animation Set
				EditorGUI.PropertyField (new Rect (pos.x, pos.y + ay, pos.width, animationsetHeight), animationset, GUIContent.none);

				if (animationset.FindPropertyRelative("folded").boolValue){
					if (EditorUtil.ex(pos.x + pos.width - EditorUtil.buttonSize, pos.y + ay, "Delete Animation Set"))
						animationsets.DeleteArrayElementAtIndex(i);
				}

				h[i] = ay;
				ay += animationsetHeight;
			}
			EditorUtil.foldLines(pos.x + EditorGUI.indentLevel * 12, pos.y, h);
			EditorGUI.indentLevel--;
		}
		return ay;
	}
}