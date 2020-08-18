using UnityEngine;
using System.Collections;

//This script is here for unit testing
//As the multiplayer version would require networking to be activated
//It does the same thing as the multiplayer version
public class AddMoreEnemy : BaseSkill {
	
	public int plusEnemy;
	
	EnemyManager[] managerScript;
	
	void Awake() {
		skillCost = 20;
		imagePath = "AMonster";
		plusEnemy = 1;
	}
	
	void Start() {
		base.Assign ();
	}

	public override void Activate () {
		playerSkill.decreaseBar (skillCost);
		GameObject spawnManager; 
		EnemyManager[] managerScripts; //we will grab enemy prefabs from here
		GameObject[] spawnPoints; //possible spawn points for the enemies
		GameObject[] enemies;//put enemy prefabs here
		float playerYPos = player.transform.position.y;
		
		//Find the opposition enemy spawn points
		if (playerYPos <= 0) {
			spawnPoints = GameObject.FindGameObjectsWithTag ("Player2EnemySpawnPoints");
			spawnManager = GameObject.FindGameObjectWithTag("EnemyManagerPlayer2");
		} 
		else {
			spawnPoints = GameObject.FindGameObjectsWithTag("Player1EnemySpawnPoints");
			spawnManager = GameObject.FindGameObjectWithTag("EnemyManagerPlayer1");
		}
		managerScripts = spawnManager.GetComponents<EnemyManager> ();
		//Grabbing enemy prefabs
		enemies = new GameObject[managerScripts.Length];
		for (int i = 0; i < managerScripts.Length; i ++) {
			enemies[i] = managerScripts[i].enemy;
		}
		
		//Randomies spawn point and enemy
		int spawnPointIndex = Random.Range (0, spawnPoints.Length); //pick a spawnpoint
		int enemyIndex = Random.Range (0, enemies.Length);

		//get no of enemies that is not on our bridge for unit testing
		int noOfEnemies = 0;
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for ( int i = 0 ; i < allEnemies.Length ; i++) {
			float yPositionOffset = allEnemies[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) {
				noOfEnemies++;
			}
		}
		//Spawn
		for (int i = 0; i < plusEnemy; i++) {
			Instantiate (enemies [enemyIndex], spawnPoints [spawnPointIndex].transform.position, Quaternion.identity); //spawn something
		}
		
		Debug.Log ("New enemy instantiated");

		OneMoreEnemyTest (noOfEnemies);
	}

	public void OneMoreEnemyTest(int noOfEnemies) {
		int currentNo = 0;
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < allEnemies.Length; i++) {
			float yPositionOffset = allEnemies[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) {
				currentNo++;
			}
		}
		if (currentNo != noOfEnemies + plusEnemy) {
			IntegrationTest.Fail (gameObject);
		}
		IntegrationTest.Pass (gameObject);
	}
	
}
