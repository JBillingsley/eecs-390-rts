using UnityEngine;
using System.Collections;

public class AnimatedEntity : MonoBehaviour {

	private Material defaultMaterial;

	public string animationSet = "Default";
	private string oldAnimationSet;
	private AnimationSet anim;
	public int animationIndex;
	private int i;

	void Start () {
		defaultMaterial = renderer.sharedMaterial;
	}
	
	void FixedUpdate () {
		updateAnimationSet ();
		i++;
		if (anim != null){
			animationIndex %= anim.animations.Length;
			Animation a = anim.animations[animationIndex];
			i %= a.tileSequence.Length * a.frameSkip;
			renderer.material.SetFloat("_Index", i / a.frameSkip);
		}
	}

	void updateAnimationSet(){
		if (!animationSet.Equals (oldAnimationSet)) {
			oldAnimationSet = animationSet;
			anim = GlobalData.getAnimationSet(animationSet);
			if (anim != null)
				renderer.material = anim.getTileset().getMaterial();
			else
				renderer.material = defaultMaterial;
		}
	}

}
