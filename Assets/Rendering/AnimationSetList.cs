using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AnimationSetList{
	
	public static AnimationSetList list = new AnimationSetList();

	public List<AnimationSet> animationsets = new List<AnimationSet>();
	[SerializeField]
	private bool animationsetFolded;

	private AnimationSetList(){
		animationsets.Add (new AnimationSet());
	}

	public static string[] getAnimationSetNames(){
		string[] names = new string[list.animationsets.Count];
		int i = 0;
		foreach (AnimationSet a in list.animationsets)
			names[i++] = a.name;
		return names;
	}

	public static string[] getAnimationNames(int animation){
		AnimationSet animationset = list.animationsets[animation];
		string[] names = new string[animationset.animations.Length];
		int i = 0;
		foreach (Animation a in animationset.animations)
			names[i++] = a.name;
		return names;
	}
	
	public static AnimationSet getAnimationSet(int i){
		if (i >= 0 && i < list.animationsets.Count)
			return list.animationsets[i];
		return null;
	}

	public static AnimationSet getAnimationSet(string name){
		foreach (AnimationSet a in list.animationsets)
			if (a.name.Equals (name))
				return a;
		return null;
	}
}
