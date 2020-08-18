using UnityEngine;
using System.Collections;

//This script controls the enemy health behaviour and also 
//increasing the player mana bar when the enemy dies.
public class EnemyHealth : MonoBehaviour {
	
	public int startingHealth;
	public int currentHealth;
	public float sinkSpeed; //when enemy dies, we will make them sink through the floor
	public int scoreValue; //how much they increase our score by
	public int skillValue; //How much they will increase the skill bar when it died
	
	GameObject player; //reference to the player game object
	Animator anim;
	bool isDead;
	PlayerSkill playerSkill; //reference to player skill script (control skill bar)
	EnemyMovement enemyMovement; //reference to enemy movement script
	EnemyAttack enemyAttack; //reference to enemy atttack script
	const float DeathDelay = 1f;
	
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerSkill = player.GetComponent <PlayerSkill> ();
		anim = GetComponent <Animator> ();
		currentHealth = startingHealth;
		enemyMovement = GetComponent <EnemyMovement> ();
		enemyAttack = GetComponent <EnemyAttack> ();
	}
	
	public void TakeDamage (int amount) 
	{
		if (isDead) {
			return;
		}

		//For unit testing
		int previousHealth = currentHealth; 
		currentHealth -= amount;

		if (currentHealth != previousHealth - amount) {
			IntegrationTest.Fail(gameObject);
		}
		IntegrationTest.Pass (gameObject);
		
		if(currentHealth <= 0)
		{
			Death ();
		}
	}
	
	
	public void Death ()
	{
		playerSkill.increaseBar (skillValue);
		isDead = true;
		enemyMovement.speed = 0;
		enemyAttack.enabled = false; // disable enemy attacking
		
		anim.SetTrigger ("IsDead");
		GetComponent <BoxCollider2D> ().enabled = false; //So that player can walk over it
		GetComponent <CircleCollider2D> ().enabled = false; //So that player wont detect it as enemy anymore
		GetComponent <AudioSource> ().Play ();
		GetComponent <Rigidbody2D> ().isKinematic = true; //so unity will ignore it
		ScoreManager.score += scoreValue; //Add score
		Destroy (gameObject, DeathDelay);
	}
}