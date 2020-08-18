using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Control enemy movement behaviour
public class EnemyMovementMult : NetworkBehaviour {

	[SyncVar]
	public float speed; //enemy movement speed
	
	Transform player;
	PlayerMovementMult playerMovement; 
	Rigidbody2D enemyRigidbody2D; 
	bool facingRight;
	Animator anim;
	const float Center = 1;
	
	void Awake() {
		//get all the players on the scene
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player"); 
		
		//Get the player that the enemy will track
		//It will track the player that is standing on the same 'bridge' 
		for (int i = 0; i < players.Length; i++) {
			float playerLocationOffset = gameObject.transform.position.y - players[i].transform.position.y;
			if ( (playerLocationOffset < Center) && (playerLocationOffset > -(Center)) ) {
				player = players[i].transform;
				playerMovement = players[i].GetComponent<PlayerMovementMult>();
			}
		}
		
		
		enemyRigidbody2D = GetComponent<Rigidbody2D> ();
		facingRight = true;
	}

	//whenever anything enter the trigger
	void OnTriggerEnter2D (Collider2D other) 
	{
		//Stop enemies from colliding with one another
		if (other.gameObject.tag == "Enemy") {
			Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),other);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		float enemyPos = transform.position.x; //enemy x position
		float playerPos = player.position.x; //player x position
		
		if (enemyPos > playerPos) { 
			Moving ("left");
		} 
		else if (enemyPos < playerPos) {
			Moving ("right");
		} 
		
	}
	
	//change the direction that the object is facing
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1; //flipping x axis
		transform.localScale = theScale;
	}
	
	//moving the enemy 
	void Moving (string direction) {
		//enemy's position calculated based off player speed and it's speed
		//sprite flipped if needed
		if (direction == "left") {
			enemyRigidbody2D.velocity = new Vector2 ( (-speed) -playerMovement.speed* playerMovement.movingRight, enemyRigidbody2D.velocity.y);
			if (!facingRight) {
				Flip ();
			}
		} 
		else if (direction == "right") {
			enemyRigidbody2D.velocity = new Vector2 (  speed - playerMovement.speed*playerMovement.movingRight, enemyRigidbody2D.velocity.y);;
			if (facingRight) {
				Flip ();
			}
		}
	}
}

