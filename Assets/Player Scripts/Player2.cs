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

	private bool climbing;

	public enum movementState {WALKING,JUMPING,IDLE,CLIMBING};
	public movementState currentState;

	private Vector3 startPosition;

	Map map;
	private TileSpec currentTile;

	// Use this for initialization
	void Start () {
		pPhysics = GetComponent<PhysicsController>();
		moveAmount = new Vector2();
		defaultXScale = transform.localScale.x;
		StartCoroutine(computeState());
		startPosition = this.transform.position;
		map = GameObject.FindObjectOfType<Map>();
	}
	
	// Update is called once per frame
	private void FixedUpdate () {
		base.FixedUpdate();
		if (pPhysics.onWall) {
			targetSpeed = 0;
			currentSpeed = 0;
		}
		targetSpeed = speed * Input.GetAxisRaw("Horizontal");

		float climb = Input.GetAxisRaw("Vertical");

		currentSpeed = IncrementSpeed(currentSpeed,targetSpeed,acceleration);
		
		moveAmount.x = currentSpeed;

		if(pPhysics.climbing){
			moveAmount.y = climb;
			if(!pPhysics.onLadder() ){//|| Mathf.Abs(currentSpeed) > .25){
				pPhysics.ungrab();
				if (Input.GetAxisRaw("Jump") > .5) {
					moveAmount.y = jumpHeight;
				}
			}
		}
		else if (pPhysics.onGround) {
			moveAmount.y = 0;
			if (Input.GetAxisRaw("Jump") > .5) {
				moveAmount.y = jumpHeight;
			}
		}
		else{
			moveAmount.y -= gravity * Time.fixedDeltaTime;
			Debug.Log (moveAmount);
			if(moveAmount.y <= -fallDeathSpeed){
				Debug.Log (moveAmount);
				reset();
			}
		}

		if(Mathf.Abs(climb) > .1f){
			pPhysics.grab();
		}

		pPhysics.move(moveAmount * Time.fixedDeltaTime);
		currentTile = map.getForeground((Vector2)this.transform.position);
		if(currentTile == TileSpecList.getTileSpec("Die")){
			reset();
		}
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
			if(pPhysics.climbing){
				currentState = movementState.CLIMBING;
			}

			switch(currentState){
			case movementState.IDLE:
				animater.animationID = 0;
				if(moveAmount.x != 0){
					currentState = movementState.WALKING;
				}
				if(!pPhysics.onGround && moveAmount.y != 0){
					currentState = movementState.JUMPING;
				}
				break;
			case movementState.JUMPING:
				animater.animationID = 2;
				if(pPhysics.onGround){
					currentState = movementState.WALKING;
				}
				break;
			case movementState.WALKING:
				animater.animationID = 1;
				if(!pPhysics.onGround && moveAmount.y != 0){
					currentState = movementState.JUMPING;
				}
				else{
					if(moveAmount.x == 0){
						currentState = movementState.IDLE;
					}
				}
				break;
			case movementState.CLIMBING:
				animater.animationID = 3;
				if(!pPhysics.climbing){
					currentState = movementState.WALKING;
				}
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
	void reset(){
		this.transform.position = startPosition;
		moveAmount = Vector2.zero;
	}
}
