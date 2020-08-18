using UnityEngine;
using System.Collections;

//This script controls the enemy attacking behaviours
public class EnemyAttack : MonoBehaviour {
	
	public int attackDamage;
	
	GameObject player; //reference to the player game object
	PlayerHealth playerHealth; //reference to the player health script
	EnemyHealth enemyHealth;
	bool playerInRange; //set to true when enemy is near the player
	float attackTimer; //How long does enemy have to wait until next attack
	bool attackCoolDown; //Indicator whether enemy is able to attack or not.
	
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent<EnemyHealth>();
		attackTimer = 2.0f;
		attackCoolDown = false;
	}
	
	void OnTriggerEnter2D (Collider2D other) //whenever anything enter the trigger
	{
		if(other.gameObject == player)
		{
			playerInRange = true;
		}
	}
	
	
	void OnTriggerExit2D (Collider2D other) //whenever anything is gone from the trigger
	{
		if(other.gameObject == player)
		{
			playerInRange = false;
		}
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		if(playerInRange && attackCoolDown == false && enemyHealth.currentHealth > 0)
		{
			attackCoolDown = true;
			Attack ();
			//Wait for a certain amount before the enemy can attack again
			StartCoroutine ("attackCooldown");
		}
	}
	
	
	void Attack () //attack timer
	{
		if(playerHealth.currentHealth > 0)
		{
			playerHealth.TakeDamage (attackDamage);
		}
	}

	//Wait for a period of time before it can attack again
	IEnumerator attackCooldown ()	
	{	
		yield return new WaitForSeconds (attackTimer);
		attackCoolDown = false;
	}
}
