using UnityEngine;
using System.Collections;

//This script controls the enemy movement behaviour
public class EnemyMovement : MonoBehaviour {
	
	public float speed;
	
	Transform player;
	PlayerMovement playerMovement;
	Rigidbody2D enemyRigidbody2D;
	bool facingRight = true;
	Animator anim;
	
	void Awake() {
		enemyRigidbody2D = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerMovement = PlayerMovement.FindObjectsOfType<PlayerMovement>()[0];
	}
	
	void OnTriggerEnter2D (Collider2D other) //whenever anything enter the trigger
	{
		//Make sure enemy cannot collide with each other
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

