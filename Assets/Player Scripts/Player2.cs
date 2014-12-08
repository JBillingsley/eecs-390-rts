using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhysicsController))]
public class Player2 : AnimatedEntity {

	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;

	public float fallDeathSpeed = 50;

	public float currentSpeed;
	public float targetSpeed;

	public Vector2 moveAmount;

	private float defaultXScale;

	private PhysicsController pPhysics;

	public enum movementState {WALKING,JUMPING,IDLE,CLIMBING};
	public movementState currentState;
	// Use this for initialization
	void Start () {
		pPhysics = GetComponent<PhysicsController>();
		moveAmount = new Vector2();
		defaultXScale = transform.localScale.x;
		StartCoroutine(computeState());
	}
	
	// Update is called once per frame
	private void FixedUpdate () {
		base.FixedUpdate();
		if (pPhysics.onWall) {
			targetSpeed = 0;
			currentSpeed = 0;
		}
		
		moveAmount.y -= gravity * Time.fixedDeltaTime;

		targetSpeed = speed * Input.GetAxisRaw("Horizontal");
		currentSpeed = IncrementSpeed(currentSpeed,targetSpeed,acceleration);
		
		moveAmount.x = currentSpeed;

		if (pPhysics.onGround) {
			moveAmount.y = 0;
			if (Input.GetAxisRaw("Jump") > .5) {
				moveAmount.y = jumpHeight;
			}
		}

		pPhysics.move(moveAmount * Time.fixedDeltaTime);
	}

	//Moves the current speed towards the target, by the acceleration amount.
	private float IncrementSpeed (float c, float t, float a){
		if(c == t){
			return t;
		}
		else{
			float dir = Mathf.Sign(t - c);
			c += a * Time.fixedDeltaTime * dir;
			return ((Mathf.Sign(t-c)) == dir) ? c : t;
		}
	}

	protected IEnumerator computeState(){

		int counter = 0;
		while(true){
			switch(currentState){
			case movementState.IDLE:
				if(moveAmount.x != 0){
					currentState = movementState.WALKING;
					animater.animationID = 1;
				}
				break;
			case movementState.JUMPING:
				if(pPhysics.onGround){
					currentState = movementState.WALKING;
					animater.animationID = 1;
				}
				break;
			case movementState.WALKING:
				if(!pPhysics.onGround && moveAmount.y != 0){
					currentState = movementState.JUMPING;
				}
				else{
					if(moveAmount.x == 0){
						currentState = movementState.IDLE;
						animater.animationID = 2;
					}
				}
				break;
			case movementState.CLIMBING:
				break;
			}
			
			if(moveAmount.x < 0){
				right = false;
			}
			if(moveAmount.x > 0){
				right = true;
			}
			yield return null;
		}
	}
}
