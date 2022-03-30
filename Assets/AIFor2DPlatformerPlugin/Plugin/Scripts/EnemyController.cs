using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	
	public bool canPatrol = true;
	public float speed = 3f;
	public float endingPositionX = 5f;
	public bool jumpingEnabled = false;
	public float jumpForce = 400f;
	public bool canSeeForward = false;
	public bool canSeeBackAndFront = false;
	public LayerMask followMask;
	public float sightDistance;
	public bool forgetTimerEnabled = false;
	public float forgetTime = 0f;


	[HideInInspector] public bool goesToLeftFirst = false;
	[HideInInspector] public bool goingToTheRight = true;
	
	private float counter = 0;
	private bool patrolling = false;
	private float startingPositionX;
	private float distance;
	private float originalStartingPositionX;

	/***********EVENTS************/

	public delegate void NoticedTargetAction();
	public static event NoticedTargetAction OnNoticedTarget;

	/*********************************/

	//Set the starting position, the distance etc...
	
	void Start(){
		if (forgetTimerEnabled == false)
        {
            counter = -100f;
            forgetTime = 0;
        }

        if (transform.position.x < endingPositionX)
            goesToLeftFirst = false;
        else
            goesToLeftFirst = true;

		counter = forgetTime;
		patrolling = canPatrol;
		
		if (transform.position.x > endingPositionX) {
			goingToTheRight = false;
			startingPositionX = transform.position.x;
			distance = startingPositionX - endingPositionX;
			originalStartingPositionX = transform.position.x;
			Flip ();
		} else if (transform.position.x < endingPositionX) {
			goingToTheRight = true;
			startingPositionX = transform.position.x;
			distance = endingPositionX - startingPositionX;
			originalStartingPositionX = transform.position.x;
		}
		
		if (speed < 0)
			speed = speed * -1f;
		
		if (jumpForce < 0)
			jumpForce = jumpForce * -1f;
	}

	//This is a timer for a countdown when the enemy can's see the target anymore
	//So after it can't see target, it waits for it.
	void ForgetTimerCountdown(){
		if (forgetTimerEnabled == true && counter > 0f) {
			counter -= Time.deltaTime;
		} else {
			counter = forgetTime;
		}
	}


	void FixedUpdate(){
		if (patrolling == true) {
			if(goingToTheRight == true){
				PatrollingRight();
			} else if( goingToTheRight == false){
				PatrollingLeft();
			}
		}
		
		if (canSeeForward) {
			ForwardCheck ();	
		} else if (canSeeBackAndFront) {
			canSeeForward = false;
			FullCheck();
		}
	}
	
	public void PatrollingRight(){
		if (distance > 0) {
			goingToTheRight = true;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);
			if(goesToLeftFirst == false){
				distance = endingPositionX - transform.position.x;
			} else {
				distance = originalStartingPositionX - transform.position.x;
			}
		} else {
			goingToTheRight = false;
			if(goesToLeftFirst == false){
				distance = transform.position.x - originalStartingPositionX;
			} else {
				distance = transform.position.x -endingPositionX;
			}
			Flip ();
		}
	}
	
	public void PatrollingLeft(){
		if (distance > 0) {
			goingToTheRight = false;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, GetComponent<Rigidbody2D> ().velocity.y);
			if(goesToLeftFirst == false){
				distance = transform.position.x - originalStartingPositionX;
			} else {
				distance = transform.position.x -endingPositionX;
			}
		} else {
			goingToTheRight = true;
			if(goesToLeftFirst == false){
				distance = endingPositionX - transform.position.x;
			} else {
				distance = originalStartingPositionX - transform.position.x;
			}

			Flip ();
		}
	}
	
	//Flip the player so it looks to the direction where it's going
	void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	//Jumping with the help of the jumping box
	void OnTriggerEnter2D(Collider2D other){
		//If it collides with a jumping box than we give y direction force to it but only if the jumpBoxes contains the jumpBox
		if (other.name == "Jump" && jumpingEnabled) {
			CommandJump();
		}
	}
	
	void CommandJump()
    {
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
	}

	void CommandTurn()
    {

    }
	
	//Follow GameObject what is in fron of it or is at the back of it
	void FullCheck(){
		//Creating Raycasts so we know what is in front of the enemy or at the back
		RaycastHit2D leftSideCheck;
		leftSideCheck = Physics2D.Raycast (transform.position, -Vector2.right, sightDistance, followMask);
		Debug.DrawRay (transform.position, -Vector2.right * sightDistance, Color.red);
		
		RaycastHit2D rightSideCheck;
		rightSideCheck = Physics2D.Raycast (transform.position, Vector2.right, sightDistance, followMask);
		Debug.DrawRay (transform.position, Vector2.right * sightDistance, Color.red);

		//When something is detected on the layer mask, the enemy doesn't patrol anymore,
		//It starts to follow the detected gameobject
		if (leftSideCheck.collider != null) {
			patrolling = false;
			if(goingToTheRight == true){
				Flip ();
				goingToTheRight = false;
			}
			
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, GetComponent<Rigidbody2D> ().velocity.y);

			//Fire the event
			if(OnNoticedTarget != null){
				OnNoticedTarget();
			}

			NoticedTarget();

		} else if (rightSideCheck.collider != null) {
			patrolling = false;
			if(goingToTheRight == false){
				Flip ();
				goingToTheRight = true;
			}
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);

			//Fire the event
			if(OnNoticedTarget != null){
				OnNoticedTarget();
			}

			NoticedTarget();

			//If we are not detecing anything than just do what we have done before the detection (patrol again or stay idle)
		} else if (leftSideCheck.collider == null && rightSideCheck.collider == null && canPatrol == true){
			ForgetTimerCountdown();
			if(counter <= 0f){
				patrolling = true;
			}
		}
	}
	
	
	//Follow GameObject what is in front of it
	/*
	 * Same as FullCheck()
	 * */
	void ForwardCheck(){
		RaycastHit2D rightSideCheck;
		if (goingToTheRight && canSeeForward) {
			rightSideCheck = Physics2D.Raycast (transform.position, Vector2.right, sightDistance, followMask);
			Debug.DrawRay (transform.position, Vector2.right * sightDistance, Color.red);
			
			if (rightSideCheck.collider != null) {
				patrolling = false;
				if(goingToTheRight == false){
					Flip ();
					goingToTheRight = true;
				}
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);

				//Fire the event
				if(OnNoticedTarget != null){
					OnNoticedTarget();
				}

				NoticedTarget();

			} else if (rightSideCheck.collider == null){
				ForgetTimerCountdown();
				if(canPatrol == true && counter <= 0f){
					patrolling = true;
				}
			}
		} else if (canSeeForward && goingToTheRight == false) {
			rightSideCheck = Physics2D.Raycast (transform.position, -Vector2.right, sightDistance, followMask);
			Debug.DrawRay (transform.position, -Vector2.right * sightDistance, Color.red);
			
			if (rightSideCheck.collider != null) {
				patrolling = false;
				if(goingToTheRight == true){
					Flip ();
					goingToTheRight = false;
				}
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, GetComponent<Rigidbody2D> ().velocity.y);

				//Fire the event
				if(OnNoticedTarget != null){
					OnNoticedTarget();
				}

				NoticedTarget();

			} else if (rightSideCheck.collider == null){
				ForgetTimerCountdown();
				if(canPatrol == true  && counter <= 0f){
					patrolling = true;
				}
			}
		}
	}

	//Sends message to the gameobject
	void NoticedTarget(){
		gameObject.SendMessage("ISeeTarget", SendMessageOptions.DontRequireReceiver);

	}
}