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

	public void selectAnimation(string name){
		int i = 0;
		foreach (Animation a in animationSet.animations) {
			if (a.name.Equals(name)){
				animationID = i;
				return;
			}
			i++;
		}
		animationID = -1;
	}

}
