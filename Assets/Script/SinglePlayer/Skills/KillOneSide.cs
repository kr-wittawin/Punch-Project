using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This skill will kill all of the enemies in the direction that the player is facing
public class KillOneSide : BaseSkill {
	
	protected GameObject[] enemy;
	protected EnemyHealth enemyHealth;
	
	void Awake() {
		skillCost = 60;
		imagePath = "RocketFist";
	}
	
	void Start() {
		base.Assign ();
	}
	
	//Kill all the enemy in the scene
	public override void Activate ()
	{
		playerSkill.decreaseBar (skillCost);

		bool playerFacing = player.GetComponent<PlayerMovement>().GetFacingRight();
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemy.Length; i++) {
			enemyHealth = enemy[i].GetComponent<EnemyHealth>();
			if (playerFacing) { //Player facingRight
				//Destroy all enemies on the right side of the player
				if (enemy[i].transform.position.x > player.transform.position.x) { 
					enemyHealth.skillValue = 0; //So that when we kill it, our mana doesnt go up
					enemyHealth.Death();
				}
			}
			else { //Player facingLeft
				//Destroy all enemies on the left side of the player
				if (enemy[i].transform.position.x < player.transform.position.x) { 
					enemyHealth.skillValue = 0; //So that when we kill it, our mana doesnt go up
					enemyHealth.Death();
				}
			}
		}
		KillOneSideTest(playerFacing);
		Debug.Log ("One side activated");
	}
	
	//To test whether the correct side has been wiped after activation
	public void KillOneSideTest(bool playerFacing) {
		GameObject[] testEnemy;
		EnemyMovement enemyMovement;
		testEnemy = GameObject.FindGameObjectsWithTag ("Enemy");
		//There should be no more enemy
		for (int i = 0; i < testEnemy.Length; i++) {
			if (playerFacing) {
				if (testEnemy[i].transform.position.x > player.transform.position.x) {
					enemyMovement = testEnemy [i].GetComponent<EnemyMovement> ();
					if (enemyMovement.speed != 0) {
						IntegrationTest.Fail (gameObject);
					}
				}
			}
			else {
				if (testEnemy[i].transform.position.x < player.transform.position.x) {
					enemyMovement = testEnemy [i].GetComponent<EnemyMovement> ();
					if (enemyMovement.speed != 0) {
						IntegrationTest.Fail (gameObject);
					}
				}
			}
			
		}
		IntegrationTest.Pass (gameObject);
	}
	
	
}