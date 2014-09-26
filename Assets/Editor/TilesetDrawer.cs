using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof (Tileset))]
public class TilesetDrawer : PropertyDrawer {

	const int texSize = 100;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		return Mathf.Max(5 * base.GetPropertyHeight (property, label), texSize);
	}

	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty width = prop.FindPropertyRelative ("width");
		SerializedProperty height = prop.FindPropertyRelative ("height");
		SerializedProperty texture = prop.FindPropertyRelative ("texture");
		float dy = base.GetPropertyHeight(prop, label);
		EditorGUIUtility.labelWidth = 75;
		EditorGUI.PropertyField (new Rect (pos.x, pos.y + dy, pos.width - texSize, dy), name, new GUIContent("Name"));
		EditorGUI.PropertyField (new Rect (pos.x, pos.y + 3*dy, pos.width - texSize, dy), width, new GUIContent("Width"));
		EditorGUI.PropertyField (new Rect (pos.x, pos.y + 4*dy, pos.width - texSize, dy), height, new GUIContent("Height"));
		EditorGUI.ObjectField(new Rect (pos.width - texSize, pos.y, texSize, texSize), "", (Texture)texture.objectReferenceValue, typeof(Texture));
	}
	
}

/*[CustomPropert
 * yDrawer(typeof(Tileset))]
public class TilesetEditor : PropertyDrawer {
	/*
	public override void OnInspectorGUI(){
		Debug.Log(target.GetType());

		Tileset t = (Tileset)target;
		
		GUI.enabled = !Application.isPlaying;
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical(GUILayout.Width(100));
		EditorGUIUtility.labelWidth = 50;
		EditorGUIUtility.fieldWidth = 50;
		EditorGUILayout.Space();EditorGUILayout.Space();
		t.setWidth(EditorGUILayout.IntField("Width", Mathf.Max(t.width, 1)));
		t.setHeight(EditorGUILayout.IntField("Height", Mathf.Max(t.height, 1)));
		EditorGUILayout.Space();EditorGUILayout.Space();
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();
		EditorGUILayout.EndVertical(); 

		EditorGUILayout.BeginVertical(new GUILayoutOption[] {GUILayout.Width(100),GUILayout.Height(100)});
		t.setTexture((Texture)EditorGUI.ObjectField(GUILayoutUtility.GetRect(30,30,100,100), t.texture, typeof(Texture), true));
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
	}
	
	

}*/