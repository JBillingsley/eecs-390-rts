using UnityEngine;
using System.Collections;

public class Maths {
	
	public static Vector3 FlatForward(Quaternion quat){
		return Vector3.Cross (Vector3.up, Left(quat));
	}

	public static Vector3 Forward(Quaternion quat){
		return quat * Vector3.forward;
	}

	public static Vector3 Left(Quaternion quat){
		return Vector3.Cross (quat * Vector3.forward, Vector3.up);
	}

	/**
	 * Gives the number of degrees away from (1, 0, 0) along the x-z plane the quaternion is facing.
	 **/

	public static float Yaw(Quaternion quat){
		return Vector3.Angle (FlatForward(quat), new Vector3(1, 0, 0));
	}

	/**
	 * Gives the number of degrees above the x-z plane the quaternion is facing.
	 **/
	public static float Pitch(Quaternion quat){
		return Vector3.Angle (FlatForward(quat), quat * Vector3.forward);
	}

	/**
	 * Gives the number of degrees that object up is away from reference up.
	 **/
	public static float Roll(Quaternion quat){
		return Vector3.Angle (Vector3.up, quat * Vector3.up);
	}

	/**
	 * Proper modulus.
	 **/
	public static float Mod(float f, float n){
		return f - n * Mathf.Floor(f / n);
	}

	/**
	 * Clamps f to within delta from reff given a cyclic range of [0, period). Result is within [0, period). 
	 **/
	public static float CyclicClamp(float f, float reff, float delta, float period){
		if (delta > period / 2)
			Debug.LogError("delta cannot be larger than period / 2 in Maths.CyclicClamp()");
		float df = Mod(f - reff, period);
		if (df <= delta)
			return Mod(f + df, period);
		if (df < period / 2)
			return Mod(f + delta, period);
		if (df < period - delta)
			return Mod(f - delta, period);
		return Mod(f + df, period);
	}

}
