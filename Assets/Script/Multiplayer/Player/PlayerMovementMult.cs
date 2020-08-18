using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// In Multiplayer, playerMovement will only be done on the server side
// In our game, instead of the player, the scene is the one that are actually 
// "sliding", thus making it looks like the player is moving
// Everytime the client push a button, it will send a request to the server
// Using [Command] to move the scene.
// The part of the scene that will moves depend on the player position

// The script also controls whether the player hit an enemy everytime it moves
// For the client part, if it hits an enemy, it will get that enemy and send
// a request to the server to apply damage to the enemy.
public class PlayerMovementMult : NetworkBehaviour {
	//Public variables
	public int punchDamage;
	public float range;
	public int movingRight;
	public float speed;

	//How long we have to wait if we miss before we can hit again
	[SyncVar]
	public float missTimer; 
	
	bool facingRight = false;
	Animator anim;
	int punchableMask; // to make sure we can only punch what is punchable
	bool punchCoolDown; // Indicator whether the player can punch/move
	Transform player;
	
	void Awake() {
		anim = GetComponent <Animator> ();
		punchableMask = LayerMask.GetMask ("Punchable");
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		punchCoolDown = false;
	}

	//Getter
	public bool GetFacingRight() {
		return facingRight;
	}
	
	// Use this for initialization
	void Start () {
		// Assign this player to the game controller
		// This is done so the gamecontroller know which is the local player
		if (isLocalPlayer) {
			Debug.Log ("I have assigned player to gamecontroller");
			GameControllerMult gameController = GameObject.FindGameObjectWithTag ("GameControllerMult").GetComponent<GameControllerMult> ();
			gameController.targetPlayer = gameObject;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		// Return if the player is not the local player
		// Done so all the movement will only affect the local player
		if (!isLocalPlayer) {
			return;
		}

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
					CmdIdleAnim ();
				}
			}
		}

		//Windows/Mac control
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
		//player not moving, change the animation back to idle
		else { 
			CmdIdleAnim();
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
			CmdMoveLevel(-1); //Move the level
			Invoke("stop",0.5f);

			//Flip the player if needed
			if (facingRight) {
				CmdFlipPlayer();
			}

			Punch (Vector2.left); //Try to punch enemy
		} 

		else if (movingRight == 1) {
			CmdMoveLevel (1); //Move the level
			Invoke("stop",0.5f);

			//Flip the player if needed
			if (!facingRight) {
				CmdFlipPlayer ();
			}

			Punch (Vector2.right); //Try to punch enemy
		}

		CmdAttackAnim (); //Change animation to attacking
	}
	
	//Punching the opponent
	void Punch (Vector2 dir) {
		Debug.Log ("PUNCHING");

		//See whether there is an object within the player attack range
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, range, punchableMask);
		player.GetComponent<AudioSource>().Play (); //attack audio

		//if we hit something
		if (hit) { 
			Debug.Log ("WE HIT ENEMY");
			GameObject enemy = hit.transform.gameObject;
			CmdDamageEnemy(enemy);
		} 

		//else our player cant move for awhile
		else 
		{
			Debug.Log("Timer Reset");
			punchCoolDown = true;
			StartCoroutine ("punchTimer");
		}
	}

	void stop(){
		CmdMoveLevel (0);
		movingRight = 0;
	}

	//Sync which enemy which will get hit
	[Command]
	void CmdDamageEnemy(GameObject targetEnemy) {
		EnemyHealthMult enemyHealth = targetEnemy.GetComponent<EnemyHealthMult> ();
		if (enemyHealth != null) { //in case we hit a wall/backgrounds which does not have health
			enemyHealth.TakeDamage (punchDamage);
		}
	}

	//Sync level movement
	[Command]
	void CmdMoveLevel(int direction) {
		ScrollManagerMult[] back = ScrollManagerMult.FindObjectsOfType<ScrollManagerMult>();
		foreach (ScrollManagerMult level in back){
			if (!level.top & this.transform.position.y < 2){
				level.ChangeDirection(direction);
			} else if (level.top & this.transform.position.y > 2) {
				level.ChangeDirection(direction);
			}
		}
	}

	//We only change animations and flipping in server side
	//And by using [ClientRpc], we sync it to the client
	//Sync flipping
	[Command]
	void CmdFlipPlayer() {
		RpcFlip ();
	}

	//Sync attack animation
	[Command]
	void CmdAttackAnim() {
		RpcAttackAnim ();
	}

	//Sync idle animation
	[Command]
	void CmdIdleAnim() {
		RpcIdleAnim ();
	}

	//Start cooldown if we miss an attack
	//During this time, the player wont be able to attack
	IEnumerator punchTimer ()
	{
		yield return new WaitForSeconds (missTimer);
		punchCoolDown = false;
	}

	//So client can see the animations as well
	[ClientRpc]
	void RpcAttackAnim() {
		anim.SetBool ("Attacking", true);
	}
	
	[ClientRpc]
	void RpcIdleAnim() {
		anim.SetBool ("Attacking", false);
	}
	
	[ClientRpc]
	void RpcFlip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1; //flipping x axis
		transform.localScale = theScale;
	}

}