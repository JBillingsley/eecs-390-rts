using UnityEngine;
using System.Collections;

public class AnimatedEntity : MonoBehaviour {

	private Material defaultMaterial;
	public AnimationSpec animation;
	private bool oldright = true;
	public bool right = true;
	private float f;
	private int i;

	void Start () {
		defaultMaterial = renderer.sharedMaterial;
	}
	
	public void FixedUpdate () {
		if (right != oldright) {
			oldright = right;
			if (right)
				GetComponent<MeshFilter>().mesh = UnitMesh.inst.rightMesh;
			else
				GetComponent<MeshFilter>().mesh = UnitMesh.inst.leftMesh;
		}
		updateAnimationSet ();
		f += Time.fixedDeltaTime * 60;
		i = Mathf.FloorToInt(f);
		Animation a = animation.animation;
		if (a != null){
			i %= a.tileSequence.Length * a.frameSkip;
			renderer.material.SetFloat("_Index", a.tileSequence[i / a.frameSkip]);
		}
		else
			Debug.Log("No Valid Animation Set");
	}

	void updateAnimationSet(){
		AnimationSet anim = animation.animationSet;
		if (anim != null)
			renderer.material = anim.getTileset() == null? null : anim.getTileset().getMaterial();
		else
			renderer.material = defaultMaterial;
	}

}
