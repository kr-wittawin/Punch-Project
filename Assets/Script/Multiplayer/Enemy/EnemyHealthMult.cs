using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Control enemy health behaviour
//And also increase player mana bar by a specific amount when the enemy is dead
public class EnemyHealthMult : NetworkBehaviour {
	
	public int startingHealth;
	
	[SyncVar]
	public int currentHealth;
	
	public float sinkSpeed; //when enemy dies, we will make them sink through the floor
	public int scoreValue; //how much they increase our score by
	public int skillValue; //How much they will increase the skill bar when it died

	Animator anim;
	bool isDead;
	PlayerSkillMult playerSkill; //reference to player skill script (control skill bar)
	EnemyMovementMult enemyMovement; //reference to enemy movement script
	EnemyAttackMult enemyAttack; //reference to enemy atttack script
	PlayerScore playerScore;
	const float Center = 1;
	
	void Awake ()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player"); //get all the players on the scene
		
		//Get the player that the enemy will track
		//It will track the player that is standing on the same 'bridge' 
		for (int i = 0; i < 2; i++) {
			float playerLocationOffset = gameObject.transform.position.y - players[i].transform.position.y;
			if ( (playerLocationOffset < Center) && (playerLocationOffset > -(Center)) ) {
				playerSkill = players[i].GetComponent<PlayerSkillMult>();
				playerScore = players[i].GetComponent<PlayerScore>();
			}
		}
		
		anim = GetComponent <Animator> ();
		currentHealth = startingHealth;
		enemyMovement = GetComponent <EnemyMovementMult> ();
		enemyAttack = GetComponent <EnemyAttackMult> ();
	}
	
	//Called by playerMovement when the player try to attack enemy
	public void TakeDamage (int amount) 
	{
		if (isDead) {
			return;
		}
		
		currentHealth -= amount;
		
		if(currentHealth <= 0)
		{
			Death ();
		}
	}
	
	public void Death ()
	{
		playerSkill.increaseBar (skillValue);
		
		isDead = true;
		enemyMovement.speed = 0; //makes enemy cant move anymore
		enemyAttack.enabled = false; // disable enemy attacking
		
		anim.SetTrigger ("IsDead");
		RpcDeathAnim ();
		RpcComponentChanges ();
		
		//Allocating scores

		playerScore.score += scoreValue;
		
		Destroy (gameObject, 1f); //Destroy the object
	}

	//Client rpc is used to make sure the client side enemies will get the changes too

	//So client can see the animation as well
	[ClientRpc]
	void RpcDeathAnim() {
		anim.SetTrigger ("IsDead");
	}
	
	//So client can walk over dead enemy too
	[ClientRpc]
	void RpcComponentChanges() {
		GetComponent <BoxCollider2D> ().enabled = false; //So that player can walk over it
		GetComponent <CircleCollider2D> ().enabled = false; //So that player wont detect it as enemy anymore
		GetComponent <AudioSource> ().Play (); //Death audio
		GetComponent <Rigidbody2D> ().isKinematic = true; //so unity will ignore it
	}
	
}