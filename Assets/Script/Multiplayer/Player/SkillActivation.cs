using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//This class contains all the activation function that will called when a skill is pressed
//All of this skills are done on the server sides
//[SyncVar] is used to mimic any changes that the skill cause to a specific variables
//ex: heals, mana reduction, increasing enemies health/damage/speed
//Because the enemies are all spawn by the server, when it is destroyed on the server
//side, the same enemies will also get destroyed on the client side.
public class SkillActivation : NetworkBehaviour {

	PlayerSkillMult playerSkill;
	PlayerHealthMult playerHealth;
	PlayerMovementMult playerMovement;
	GameObject[] enemy;
	EnemyHealthMult enemyHealth;
	EnemyAttackMult enemyAttack;
	EnemyMovementMult enemyMovement;


	// Use this for initialization
	void Start () {

		playerHealth = gameObject.GetComponent<PlayerHealthMult> ();
		playerSkill = gameObject.GetComponent<PlayerSkillMult> ();
		playerMovement = gameObject.GetComponent<PlayerMovementMult> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Heal the player by a percentage of its  starting health
	[Command]
	public void CmdPotionSkill(float healPercentage, int skillCost) {
		Debug.Log ("POTION ACTIVATED");
		int previousHealth = playerHealth.currentHealth; //For unit testing
		int healAmount = (int)(playerHealth.startingHealth * (healPercentage / 100));
		
		playerSkill.decreaseBar (skillCost);
		playerHealth.Heal (healAmount);
		
		
		//Check whether we heal correct amount of health
		if (playerHealth.currentHealth == (previousHealth + healAmount)) {
			IntegrationTest.Pass (gameObject);
		} 
		else if (playerHealth.currentHealth != (previousHealth + healAmount)) {
			IntegrationTest.Fail (gameObject);
		}
	}

	//Kill all the enemies on the side that the player is facing
	//Only kill the enemies on the player bridge
	[Command]
	public void CmdKillOneSideSKill (int skillCost)
	{
		playerSkill.decreaseBar (skillCost);
		bool playerFacing = playerMovement.GetFacingRight ();
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - gameObject.transform.position.y;
			enemyHealth = enemy[i].GetComponent<EnemyHealthMult>();
			if (playerFacing) { //Player facingRight
				//Destroy all enemies on the right side of the player
				if (enemy[i].transform.position.x > gameObject.transform.position.x) {
					if (yPositionOffset > -1 && yPositionOffset < 1) { //Kill enemy that is on the player bridge
						enemyHealth.skillValue = 0; //So that when we kill it, our mana doesnt go up
						enemyHealth.Death();
					}
				}
			}
			else { //Player facingLeft
				//Destroy all enemies on the left side of the player
				if (enemy[i].transform.position.x < gameObject.transform.position.x) { 
					if (yPositionOffset > -1 && yPositionOffset < 1) { //Kill enemy that is on the player bridge
						enemyHealth.skillValue = 0; //So that when we kill it, our mana doesnt go up
						enemyHealth.Death();
					}
				}
			}
		}

		Debug.Log ("One side activated");
	}

	//Kill all the enemy that is on the player bridge
	[Command]
	public void CmdKillAllSkill (int skillCost)
	{
		playerSkill.decreaseBar (skillCost);
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - gameObject.transform.position.y;
			enemyHealth = enemy[i].GetComponent<EnemyHealthMult>();
			if (yPositionOffset > -1 && yPositionOffset < 1) {
				enemyHealth.skillValue = 0; //So that when we kill it, our mana doesnt go up
				enemyHealth.Death();
			}
		}
		Debug.Log ("Kill All activated");
	}

	//heal all the enemies on the opposition player bridge 
	[Command]
	public void CmdHealEnemy(int skillCost) {
		playerSkill.decreaseBar (skillCost);
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		int[] previousHealth = new int[enemy.Length]; //For unit testing
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - gameObject.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) { //We heal the enemy that is not on the player bridge
				enemyHealth = enemy[i].GetComponent<EnemyHealthMult>();
				Debug.Log (enemy[i].transform.position.y);
				previousHealth[i] = enemyHealth.currentHealth; //unit testing
				enemyHealth.currentHealth = enemyHealth.startingHealth;
			}
		}
		Debug.Log ("Enemy Healed");
	}

	//Increase the damage of all the enemies on the opposition player bridge
	[Command]
	public void CmdIncreaseEnemyDamage(int increasedAmount , int skillCost) {
		playerSkill.decreaseBar (skillCost);
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - gameObject.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) { //We heal the enemy on the opposition bridge
				enemyAttack = enemy[i].GetComponent<EnemyAttackMult>();
				enemyAttack.attackDamage += increasedAmount;
			}
		}
		Debug.Log ("Enemy Attack Increased");
	}
	
	//speed up all enemies currently on the opposition bridge
	[Command]
	public void CmdSpeedUpEnemies(int skillCost, int plusSpeed) {
		playerSkill.decreaseBar (skillCost);
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - gameObject.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) { //We speed up the enemy on the opposition bridge
				enemyMovement = enemy[i].GetComponent<EnemyMovementMult>();
				enemyMovement.speed += plusSpeed;
			}
		}
		Debug.Log ("Enemy Speed Increased");

	}

	//Spawn one random enemy to the enemy
	[Command]
	public void CmdAddMoreEnemy(int skillCost, int plusEnemy){
		playerSkill.decreaseBar (skillCost);
		GameObject spawnManager; 
		EnemyManagerMult[] managerScripts; //we will grab enemy prefabs from here
		GameObject[] spawnPoints; //possible spawn points for the enemies
		GameObject[] enemies;//put enemy prefabs here
		float playerYPos = gameObject.transform.position.y;

		//Find the opposition enemy spawn points
		if (playerYPos == 0) {
			spawnPoints = GameObject.FindGameObjectsWithTag ("Player2EnemySpawnPoints");
			spawnManager = GameObject.FindGameObjectWithTag("EnemyManagerPlayer2");
		} 
		else {
			spawnPoints = GameObject.FindGameObjectsWithTag("Player1EnemySpawnPoints");
			spawnManager = GameObject.FindGameObjectWithTag("EnemyManagerPlayer1");
		}
		managerScripts = spawnManager.GetComponents<EnemyManagerMult> ();

		//Grabbing enemy prefabs
		enemies = new GameObject[managerScripts.Length];
		for (int i = 0; i < managerScripts.Length; i ++) {
			enemies[i] = managerScripts[i].enemy;
		}

		//Randomies spawn point and enemy
		int spawnPointIndex = Random.Range (0, spawnPoints.Length); //pick a spawnpoint
		int enemyIndex = Random.Range (0, enemies.Length);

		//Spawn
		for (int i = 0; i < plusEnemy; i++) {
			GameObject newEnemy = Instantiate 
				(enemies [enemyIndex], spawnPoints [spawnPointIndex].transform.position, Quaternion.identity) as GameObject; //spawn something

			NetworkServer.Spawn (newEnemy);
		}

		Debug.Log ("New enemy instantiated");
	}

	//Remove miss cooldown timer for a period of time
	[Command]
	public void CmdRemoveCooldownSkill (int skillCost, float reduceTime)
	{
		playerSkill.decreaseBar (skillCost);
		
		float originalTimer = playerMovement.missTimer;
		playerMovement.missTimer -= originalTimer;
		
		Debug.Log ("MissTimerSKill Activate");
		StartCoroutine (timerReduced(originalTimer, reduceTime));
		
		
	}

	//Wait for a period of time, then return the plaeyr miss cooldown to normal
	IEnumerator timerReduced (float originalTimer, float reduceTime)
	{
		yield return new WaitForSeconds (reduceTime);
		playerMovement.missTimer = originalTimer;
	}


	
}
