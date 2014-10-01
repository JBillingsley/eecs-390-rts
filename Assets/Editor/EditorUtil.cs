using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorUtil{
	
	public const float padding = 8;
	public const float row = 20;
	public const float height = 16;
	public const float buttonSize = 20;
	public const float texSize = 100;
	private static Texture2D line = new Texture2D(1, 1, TextureFormat.ARGB32, true);
	private static GUIStyle lineStyle = new GUIStyle();

	static EditorUtil(){
		lineStyle.normal.background = line;
	}

	public static bool plus(float x, float y, string hint){
		return GUI.Button (new Rect (x, y, buttonSize, buttonSize), new GUIContent ("", hint), (GUIStyle)"OL Plus");
	}

	public static bool minus(float x, float y, string hint){
		return GUI.Button (new Rect (x, y, buttonSize, buttonSize), new GUIContent ("", hint), (GUIStyle)"OL Minus");
	}

	public static bool ex(float x, float y, string hint){
		return GUI.Button (new Rect (x, y, buttonSize, buttonSize), new GUIContent ("", hint), (GUIStyle)"WinBtnCloseWin");
	}
	
	public static void folder(float x, float y, SerializedProperty folded){
		if (folded.boolValue){
			if (plus(x, y, "Expand"))
				folded.boolValue = false;
		}
		else{
			if (minus(x, y, "Collapse"))
				folded.boolValue = true;
		}
	}

	public static void foldLines(float x, float y, float[] i){
		if (i.Length == 0)
			return;
		float last = i[i.Length-1];
		EditorUtil.drawVLine(x - 5, y + 16, y + last + 8);
		foreach(float h in i)
			EditorUtil.drawHLine(x - 4, x, y + h + 8);
	}
	
	public static void textField(Rect rect, SerializedProperty text, bool editable, string hint){
		if (editable)
			EditorGUI.PropertyField (rect, text, new GUIContent ("", hint));
		else
			EditorGUI.LabelField (rect, new GUIContent (text.stringValue, hint));
	}

	public static void textField(Rect rect, SerializedProperty text, bool editable){
		textField (rect, text, editable, "");
	}

	public static void drawVLine(float x, float y1, float y2, Color color, float thickness){
		line.SetPixel(0, 0, color);
		line.Apply();
		GUI.Box(new Rect(x, y1, thickness, y2 - y1 + 1), GUIContent.none, lineStyle);
	}
	
	public static void drawHLine(float x1, float x2, float y, Color color, float thickness){
		line.SetPixel(0, 0, color);
		line.Apply();
		GUI.Box(new Rect(x1, y, x2 - x1 + 1, thickness), GUIContent.none, lineStyle);
	}

	public static void drawVLine(float x, float y1, float y2, Color color){
		drawVLine (x, y1, y2, color, 1);
	}

	public static void drawHLine(float x1, float x2, float y, Color color){
		drawHLine (x1, x2, y, color, 1);
	}
	
	public static void drawVLine(float x, float y1, float y2){
		drawVLine (x, y1, y2, Color.grey);
	}
	
	public static void drawHLine(float x1, float x2, float y){
		drawHLine (x1, x2, y, Color.grey);
	}
}
