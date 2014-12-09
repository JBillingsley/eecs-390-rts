using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (TileRender))]
public class TileRenderDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return calculateHeight(prop);
	}
	
	public static float calculateHeight(SerializedProperty prop){
		SerializedProperty context = prop.FindPropertyRelative ("context");
		float p = TileRenderDrawer.previewSize((TileContext)context.enumValueIndex);
		return Mathf.Max (p + EditorUtil.padding, 2 * EditorUtil.row + 3);
	}

	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		SerializedProperty context = prop.FindPropertyRelative ("context");
		SerializedProperty index = prop.FindPropertyRelative ("index");

		float p = TileRenderDrawer.previewSize((TileContext)context.enumValueIndex);
		float h = Mathf.Max (p + EditorUtil.padding, 2 * EditorUtil.row + 3);
		float w = pos.width;

		GUI.Box(new Rect (pos.x + 25, pos.y, w - 25, h), GUIContent.none);

		//
		EditorGUIUtility.labelWidth = 100;

		EditorGUI.BeginChangeCheck();

		EditorGUI.PropertyField (new Rect (pos.x, pos.y + h/2 - EditorUtil.row + 2, w - p - EditorUtil.padding, EditorUtil.height), context, new GUIContent("Context"));
		EditorGUI.PropertyField (new Rect (pos.x, pos.y + h/2 + 2, w - p - EditorUtil.padding, EditorUtil.height), index, new GUIContent("Index"));

		if (EditorGUI.EndChangeCheck ()) {
			prop.FindPropertyRelative("view").objectReferenceValue = TileRenderDrawer.constructPreview(prop);
		}

		Rect r = new Rect (pos.x + pos.width - p - 4, pos.y + 4 + (h - p - EditorUtil.padding)/2, p, p);
		Texture2D tex = (Texture2D)prop.FindPropertyRelative("view").objectReferenceValue;
		GUI.Box(r, tex == null? new GUIContent("Error"):new GUIContent(tex));
		
		EditorGUIUtility.labelWidth = 0;

		prop.serializedObject.ApplyModifiedProperties ();
	}



	
	public static Texture2D constructPreview(SerializedProperty spec){
		TextureAtlas atlas = TileSpecList.list.tileset;
		
		Texture2D tex = (Texture2D)spec.FindPropertyRelative ("view").objectReferenceValue;
		int index = spec.FindPropertyRelative ("index").intValue;
		
		switch ((TileContext)spec.FindPropertyRelative ("context").enumValueIndex) {
		case TileContext.None:
			tex = fillTexture(tex, 1, 1, atlas, new int[]{index});
			break;
		case TileContext.PartialContext:
			tex = fillTexture(tex, 3, 3, atlas, new int[]{
				index, index + 1, index + 2, 
				index + 4, index + 5, index + 6, 
				index + 8, index + 9, index + 10});
			break;
		case TileContext.FullContext:
			tex = fillTexture(tex, 5, 5, atlas, new int[]{
				index + 4, index + 34, index + 1, index + 33, index + 6, 
				index + 36, index + 22, index + 11, index + 20, index + 39, 
				index + 8, index + 25, index + 9, index + 25, index + 10,
				index + 40, index + 6, index + 11, index + 4, index + 43,
				index + 20, index + 46, index + 17, index + 45, index + 22});
			break;
		case TileContext.Slope:
			return null;
		}
		spec.FindPropertyRelative("view").objectReferenceValue = tex;
		spec.serializedObject.ApplyModifiedProperties ();
		return tex;
	}
	
	private static Texture2D fillTexture(Texture2D texture, int width, int height, TextureAtlas atlas, int[] indices){
		int w = (int)atlas.pixelWidth();
		int h = (int)atlas.pixelHeight();
		texture = ensureSize(texture, width * w, height * h);
		for (int i = 0; i < width * height; i++)
			texture.SetPixels(i % width * w, (height - (i / width) - 1) * h, w, h, atlas.tileData(indices[i]));
		texture.Apply ();
		return texture;
	}
	
	private static Texture2D ensureSize(Texture2D texture, int width, int height){
		if (texture == null) {
			texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
		}
		else if (texture.width != width || texture.height != height) {
			Texture2D.Destroy(texture);
			texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
		}
		return texture;
	}
	
	public static float previewSize(TileContext context){
		switch (context) {
		case TileContext.None:
			return 16;
		case TileContext.PartialContext:
			return 48;
		case TileContext.FullContext:
			return 80;
		case TileContext.Slope:
			return 48;
		}
		return 0;
	}
	



}
