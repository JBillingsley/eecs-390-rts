using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimationSpec {
	
	public int animationsetID;
	public int animationID;

	public AnimationSet animationSet {
		get{
			return AnimationSetList.getAnimationSet(animationsetID);
		}
	}
	public Animation animation {
		get{
			if (animationSet == null)
				return null;
			if (animationID < 0 || animationID >= animationSet.animations.Length)
				return null;
			return animationSet.animations[animationID];
		}
	}

}
