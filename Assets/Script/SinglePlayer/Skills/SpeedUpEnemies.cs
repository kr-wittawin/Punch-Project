using UnityEngine;
using System.Collections;

//This script is here for unit testing
//As the multiplayer version would require networking to be activated
//It does the same thing as the multiplayer version
public class SpeedUpEnemies : BaseSkill {
	
	public float plusSpeed;

	protected GameObject[] enemy;
	protected EnemyMovement enemyMovement;

	
	void Awake() {
		skillCost = 50;
		plusSpeed = 1;
		imagePath = "MonsterWave";
	}
	
	void Start() {
		base.Assign ();
	}
	
	
	public override void Activate () {
		playerSkill.decreaseBar (skillCost);
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		float[] previousSpeed = new float[enemy.Length]; //For unit testing
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) { //We speed up the enemy on the opposition bridge
				enemyMovement = enemy[i].GetComponent<EnemyMovement>();
				previousSpeed[i] = enemyMovement.speed;
				enemyMovement.speed += plusSpeed;
			}
		}
		Debug.Log ("Enemy Speed Increased");
		SpeedUpEnemiesTest (previousSpeed);
	}

	//To test whether enemies have their speed increased 
	public void SpeedUpEnemiesTest(float[] previousSpeed) {
		for (int i = 0; i < enemy.Length; i++) {
			enemyMovement = enemy[i].GetComponent<EnemyMovement>();
			float yPositionOffset = enemy[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) {
				if (enemyMovement.speed != previousSpeed[i] + plusSpeed) {
					IntegrationTest.Fail (gameObject);
				}
			}
		}
		IntegrationTest.Pass (gameObject);
	}
}
