using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Control the enemy attacking logic
//After an enemy has attacked, it has to wait for a period of time before
//it can attack again.
public class EnemyAttackMult : NetworkBehaviour {
	
	[SyncVar]
	public int attackDamage;
	
	GameObject player; //reference to the player game object
	PlayerHealthMult playerHealth; //reference to the player health script
	EnemyHealthMult enemyHealth; //reference to the enemy health script
	bool playerInRange; //set to true when enemy is near the player
	float attackTimer; //How long does enemy have to wait until next attack
	bool attackCoolDown; //Indicator whether enemy is able to attack or not.
	
	void Awake ()
	{
		//get all the players on the scene
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player"); 
		
		//Get the player that the enemy will track
		//It will track the player that is standing on the same 'bridge' 
		for (int i = 0; i < players.Length; i++) {
			float playerLocationOffset = gameObject.transform.position.y - players[i].transform.position.y;
			if ( (playerLocationOffset < 1) && (playerLocationOffset > -1) ) {
				player = players[i];
				playerHealth = players[i].GetComponent<PlayerHealthMult>();
			}
		}
		enemyHealth = GetComponent<EnemyHealthMult>();
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
	
	IEnumerator attackCooldown ()	
	{	
		for (int x = 1; x < 2; x++)			
		{
			yield return new WaitForSeconds (attackTimer);
			attackCoolDown = false;
		}
	}
}