using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour

{
	public GameObject enemy; 		//reference to the enemy
	public Transform[] spawnPoints; //reference to enemy spawn points
		
	public float startWait; 		// time before enemies spawn when scene begins
	public float timeBetweenWaves; 	// time between waves before enemies start spawning again
	public float timeBetweenEnemy; 	// time between each enemy spawned
	public int numSpawnWaves;	    // total number of enemy waves to be spawned, if 0 then infiniate spawning
	public int enemyPerWave; 		// number of enemies to spawn per wave
	public Transform followTarget; 	// spawn points follow this target 
	public float spawnPointOffset; 	// spawn position offset from player

	PlayerHealth playerHealth;		// current player health
	int currentNumWaves;			// current number of already spawned waves 
	
	void Start ()
	{
		// get player health, used to end spawning of enemies when player dies
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		// start spawning enemies
		StartCoroutine(SpawnWaves());
	}
	
	
	//void Spawn ()
	IEnumerator SpawnWaves ()
	{
		// initial wait before waves begin
		yield return new WaitForSeconds (startWait);
		
		currentNumWaves = 0; //how many waves have been spawn

		//Continue to spawn enemies until the player's health reach zero or there are no more waves
		while (true)
		{
			currentNumWaves++;
			// spawn enemies in regard to the number of enemies that wave
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

			// stop spawning if player dead
			if(playerHealth.currentHealth <= 0f) 
					break;

			// infinite spawn, skips next break
			if (numSpawnWaves == 0) {
				continue;
			}

			// stop spawning if last wave
			if (currentNumWaves >= numSpawnWaves)
				break;

		}
	}

	// Spawn enemy at a random spawn point
	void spawnEnemy() {
		int spawnPointIndex = Random.Range (0, spawnPoints.Length); //pick a spawnpoint
		GameObject newEnemy = Instantiate (enemy, spawnPoints[spawnPointIndex].position,Quaternion.identity) as GameObject; //spawn enemy
		newEnemy.GetComponent<EnemyAttack> ().attackDamage += (currentNumWaves -1); //Increase attack of enemy as more waves come
	}

	//Spawn point is set so that its out of the camera reach, which means that the player will not be able
	//to see the moment the enemy spawn
	//Use if needed.
	void updateSpawnPoint (int spawnPointIndex)
	{
		float x = followTarget.position.x + spawnPointOffset;
		float y = spawnPoints[spawnPointIndex].position.y;
		float z = spawnPoints[spawnPointIndex].position.z;
		spawnPoints[spawnPointIndex].position = new Vector3(x, y, z); 
	}
}
