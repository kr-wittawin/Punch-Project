using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This script is here for unit testing
//As the multiplayer version would require networking to be activated
//It does the same thing as the multiplayer version
public class HealEnemy : BaseSkill {
	
	public int healAmount;
	
	protected GameObject[] enemy;
	protected EnemyHealth enemyHealth;
	
	void Awake() {
		imagePath = "EnemyPotion";
		skillCost = 90;
	}
	
	void Start() {
		base.Assign ();
	}
	
	
	//Kill all the enemy in the scene
	public override void Activate ()
	{
		playerSkill.decreaseBar (skillCost);
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		int[] previousHealth = new int[enemy.Length]; //For unit testing
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) { //We heal the enemy that is not on the player bridge
				enemyHealth = enemy[i].GetComponent<EnemyHealth>();
				previousHealth[i] = enemyHealth.currentHealth; //unit testing
				enemyHealth.currentHealth = enemyHealth.startingHealth;
			}
		}
		Debug.Log ("Enemy Healed");

		HealEnemyTest (previousHealth);
		
	}
	
	//To test whether we heal the correct amount 
	public void HealEnemyTest(int[] previousHealth) {
		for (int i = 0; i < enemy.Length; i++) {
			enemyHealth = enemy[i].GetComponent<EnemyHealth>();
			float yPositionOffset = enemy[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) {
				if (enemyHealth.currentHealth != previousHealth[i] + healAmount) {
					IntegrationTest.Fail (gameObject);
				}
			}
		}
		IntegrationTest.Pass (gameObject);
	}
	
	
}