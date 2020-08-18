using UnityEngine;
using System.Collections;

//This script controls the player movement behaviour
//In our game, the scene is the one that scrolls, making it looks
//like the player is moving
//This script also handles whether the player hit an enemy when it moves
public class PlayerMovement : MonoBehaviour {
	//Public variables
	public int punchDamage;
	public float range;
	public int movingRight;
	public float missTimer; //How long we have to wait if we miss before we can hit again
	public float speed;
	
	bool facingRight = false;
	Animator anim;
	int punchableMask; // to make sure we can only punch what is punchable
	bool punchCoolDown; // Indicator whether the player can punch/move
	Transform player;
	const float MovementDelay = 0.5f;
	
	void Awake() {
		anim = GetComponent <Animator> ();
		punchableMask = LayerMask.GetMask ("Punchable");
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		punchCoolDown = false;
	}
	
	public bool GetFacingRight() {
		return facingRight;
	}
		
	// Update is called once per frame
	void Update () {

		//Touch screen control
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				//Move right
				if (touch.position.x > Screen.width/2 && punchCoolDown == false){
					movingRight = 1;
					Moving ();
				}
				//Move left
				else if (touch.position.x < Screen.width/2 && punchCoolDown == false){
					movingRight = -1;
					Moving ();
				} 
				//player not moving, change the animation back to idle
				else {
					anim.SetBool ("Attacking", false);
				}
			}
		}

		//Mac/Windows control
		//Move left
		if (Input.GetButtonDown ("moveLeft") && punchCoolDown == false) {
			movingRight = -1;
			Moving ();
		} 
		//Move right
		else if (Input.GetButtonDown ("moveRight") && punchCoolDown == false) {
			movingRight = 1;
			Moving ();
		} 
		else { //we are not moving, change the animation back to idle
			anim.SetBool ("Attacking", false);
		}
		
	}
	
	//change the direction that the object is facing
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1; //flipping x axis
		transform.localScale = theScale;
	}
	
	//moving the player + punching animation
	void Moving () {
		if (movingRight == -1) {
			ScrollManager[] back = ScrollManager.FindObjectsOfType<ScrollManager>();
			foreach (ScrollManager level in back){
				level.ChangeDirection(-1);
			}
			Invoke("stop",MovementDelay);
			if (facingRight) {
				Flip ();
			}
			Punch (Vector2.left); 
		} 
		else if (movingRight == 1) {
			ScrollManager[] back = ScrollManager.FindObjectsOfType<ScrollManager>();
			foreach (ScrollManager level in back){
				level.ChangeDirection(1);
			};
			Invoke("stop",MovementDelay);
			if (!facingRight) {
				Flip ();
			}
			Punch (Vector2.right);
		}
		anim.SetBool ("Attacking", true);
	}
	
	//Punching the opponent
	public void Punch (Vector2 dir) {
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, range, punchableMask);
		player.GetComponent<AudioSource>().Play ();
		if (hit) { //if we hit something
			EnemyHealth enemyHealth = hit.collider.GetComponent <EnemyHealth> (); //get the enemy health
			if (enemyHealth != null) { //in case we hit a wall/backgrounds which does not have health
				enemyHealth.TakeDamage (punchDamage);
			}
		} 
		else //else our player cant move for awhile
		{
			punchCoolDown = true;
			StartCoroutine ("punchTimer");
		}
	}
	
	void stop(){
		ScrollManager[] back = ScrollManager.FindObjectsOfType<ScrollManager>();
		foreach (ScrollManager level in back){
			level.ChangeDirection(0);
		};
		movingRight = 0;
	}
	
	//Start cooldown when player does not hit anything when moving
	//During this time, the player cant move.
	IEnumerator punchTimer ()
	{
		yield return new WaitForSeconds (missTimer);
		punchCoolDown = false;
	}
}
