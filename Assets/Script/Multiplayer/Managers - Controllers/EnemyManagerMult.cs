using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//In multiplayer, enemy spawning will entirely be done by server.
public class EnemyManagerMult : NetworkBehaviour
	
{
	public GameObject enemy; 		 //reference to the enemy
	public GameObject[] spawnPoints; //gameobject that will holds all the spawn points
	
	public float startWait;			 // time before enemies spawn when scene begins
	public float timeBetweenWaves; 	 // time between waves before enemies start spawning again
	public float timeBetweenEnemy;   // time between each enemy spawned
	public int numSpawnWaves;	     // total number of enemy waves to be spawned, if 0 then inifinate spawning
	public int enemyPerWave;		 // number of enemies to spawn per wave
	public Transform followTarget;   // spawn points follow this target
	public float spawnPointOffset;	 // spawn position offset from player
	
	int currentNumWaves;			 // current number of already spawned waves
	
	void Start ()
	{
		// start spawning enemies
		StartCoroutine(SpawnWaves());
	}
	
	//void Spawn ()
	IEnumerator SpawnWaves ()
	{
		// initial wait before waves begin
		yield return new WaitForSeconds (startWait);
		
		currentNumWaves = 0;
		
		//Continue to spawn enemies until the player's health reach zero or there are no more waves
		while (true)
		{
			currentNumWaves++;
			// spawn eneimes in regard to the number of enemies that wave 
			for (int i = 0; i < enemyPerWave ; i++)
			{
				spawnEnemy ();
				//Wait for a certain amount of time until the next enemy spawn
				yield return new WaitForSeconds (timeBetweenEnemy);
			}

			//Wait for a certain amount of time until the wave starts
			yield return new WaitForSeconds (timeBetweenWaves);

			// Increase number of enemies for next wave
			if (enemyPerWave < 10) 
				enemyPerWave++;

			// inifinite spawn, skips next break
			if (numSpawnWaves == 0) {
				continue;
			}

			// stop spawning if last wave
			if (currentNumWaves >= numSpawnWaves)
				break;
		}
	}


	// spawn enemy at a random spawn point
	void spawnEnemy() {
		int spawnPointIndex = Random.Range (0, spawnPoints.Length); //pick a spawnpoint
		GameObject newEnemy = Instantiate 
			(enemy, spawnPoints[spawnPointIndex].transform.position,Quaternion.identity) as GameObject; //spawn something
		newEnemy.GetComponent<EnemyAttackMult> ().attackDamage += (currentNumWaves -1); //Increase attack of enemy as more waves come

		//Spawn the enemy on both server and client
		NetworkServer.Spawn (newEnemy);
	}
	
	//Spawn point is set so that its out of the camera reach, which means that the player will not be able
	//to see the moment the enemy spawn
	//Not used?
	void updateSpawnPoint (int spawnPointIndex)
	{
		float x = followTarget.position.x + spawnPointOffset;
		float y = spawnPoints[spawnPointIndex].transform.position.y;
		float z = spawnPoints[spawnPointIndex].transform.position.z;
		spawnPoints[spawnPointIndex].transform.position = new Vector3(x, y, z); 
	}
}
