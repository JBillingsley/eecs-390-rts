using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PhysicsController : MonoBehaviour {

	public LayerMask collisionMask;

	private BoxCollider myCollider;
	private Vector2 s;
	private Vector2 c;

	public bool onGround;
	public bool onWall;
	public bool climbing;

	private float gap = .01f;
	public float verticalGap = 0f;

	Ray ray;
	RaycastHit hit;
	
	private Map map;

	IVector2 position;

	void Start(){
		myCollider = GetComponent<BoxCollider>();
		s = myCollider.bounds.size;
		c = myCollider.center;
		map = GameObject.FindObjectOfType<Map>();
	}

	//Moves the player the given amount, within constraints
	public void move(Vector2 moveAmount){
		float deltay = moveAmount.y;
		float deltax = moveAmount.x;
		Vector3 p = transform.position;

		onGround = false;
		onWall = false;

		for(int i = 0; i <= 1; i++){
			float dir = Mathf.Sign(deltay);
			if(deltay == 0){
				dir = -1;
			}
			float x = p.x + (c.x - s.x/2 + i * s.x) * transform.localScale.x;
			float y = p.y + c.y + (dir * (s.y/2)) * transform.localScale.y;
			Vector2 o = new Vector2(x,y);
			
			ray = new Ray(o,new Vector2(0,dir));
		
			Physics.Raycast(ray,out hit,Mathf.Abs (deltay)+gap,collisionMask);

			Debug.DrawRay(new Vector3(ray.origin.x,ray.origin.y,0),new Vector3(ray.direction.x,ray.direction.y,0));
			 
			if(hit.collider != null){
				float dist = Vector2.Distance( o,hit.point);

				if(dist > gap){
					deltay = (dist * dir) - gap * dir;
				}
				else{
					deltay = 0;
				}
				if(dir < 0){
					onGround = true;
				}
			}
		}

		//X direction
		for(int i = 0; i <= 1; i++){
			float dir = Mathf.Sign(deltax);

			float x = p.x + c.x + (dir * s.x/2); //* transform.localScale.x; //Only works because i flip local scale.
			float y = p.y + c.y - s.y/2 + (i * (s.y));// * transform.localScale.y;

			Vector2 o = new Vector2(x,y);
			
			ray = new Ray(o,new Vector2(dir,0));
			
			Physics.Raycast(ray,out hit,Mathf.Abs (deltax)+gap,collisionMask);
			
			//Debug.DrawRay(new Vector3(ray.origin.x,ray.origin.y,0),new Vector3(ray.direction.x,ray.direction.y,0));
			
			if(hit.collider != null){
				float dist = Vector2.Distance( o,hit.point);
				
				if(dist > gap){
					deltax = (dist * dir) - gap * dir;
				}
				else{
					deltax = 0;
				}
				onWall = true;
			}
		}

		//Diagonally
		if(!onGround && !onWall){

			Vector2 playerDirection = new Vector2(deltax, deltay);
			Vector2 origin = new Vector2(p.x + c.x + (Mathf.Sign(deltax) * s.x/2) * transform.localScale.x,
			                             p.y + c.y + (Mathf.Sign(deltay) * s.y/2) * transform.localScale.y);

			//Debug.DrawRay(new Vector3(origin.x,origin.y,0),
			             // new Vector3(playerDirection.x,playerDirection.y,0).normalized);

			ray = new Ray(origin,playerDirection.normalized);
			Physics.Raycast(ray,out hit,Mathf.Abs (deltay)+gap,collisionMask);
			Debug.DrawRay(new Vector3(ray.origin.x,ray.origin.y,0),new Vector3(ray.direction.x,ray.direction.y,0));

			if(hit.collider != null){
				float dist = Vector2.Distance( ray.origin,hit.point);
				
				if(dist > gap){
					deltax = 0;//(dist * playerDirection.normalized.x) - gap * playerDirection.normalized.x;
					deltay = 0;//(dist * playerDirection.normalized.y) - gap * playerDirection.normalized.y;
				}
				else{
					Vector3 point = hit.point;
					deltax = 0;
					deltay = 0;
				}
				onWall = true;
				deltax = hit.point.x - p.x;
				//onGround = true;
			}

		}

		transform.Translate (new Vector3(deltax,deltay,0));
		position = (IVector2)((Vector2)transform.position);
	}

	public void grab ()
	{
		int i = TileSpecList.getTileSpecInt("Ladder");
		if(map.getForeground(position).index == i){
			onGround = true;
		}
	}
}
