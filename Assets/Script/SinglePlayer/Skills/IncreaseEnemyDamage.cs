using UnityEngine;
using System.Collections;

//This script is here for unit testing
//As the multiplayer version would require networking to be activated
//It does the same thing as the multiplayer version
public class IncreaseEnemyDamage : BaseSkill {
	
	public int increasedAmount;
	
	protected GameObject[] enemy;
	protected EnemyAttack enemyAttack;
	
	void Awake() {
		imagePath = "DoubleAxe";
		skillCost = 80;
		increasedAmount = 10;
	}
	
	void Start() {
		base.Assign ();
	}
	
	
	void update() {
		base.Update ();
	}
	
	//Kill all the enemy in the scene
	public override void Activate ()
	{
		playerSkill.decreaseBar (skillCost);
		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		int[] previousDamage = new int[enemy.Length]; //For unit testing
		for (int i = 0; i < enemy.Length; i++) {
			float yPositionOffset = enemy[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) { //We heal the enemy on the opposition bridge
				enemyAttack = enemy[i].GetComponent<EnemyAttack>();
				previousDamage[i] = enemyAttack.attackDamage;
				enemyAttack.attackDamage += increasedAmount;
			}
		}
		Debug.Log ("Enemy Attack Increased");

		IncreaseEnemyAttackTest (previousDamage);
	}

	public void IncreaseEnemyAttackTest(int[] previousDamage) {
		for (int i = 0; i < enemy.Length; i++) {
			enemyAttack = enemy[i].GetComponent<EnemyAttack>();
			float yPositionOffset = enemy[i].transform.position.y - player.transform.position.y;
			if (yPositionOffset > 1 || yPositionOffset < -1) {
				if (enemyAttack.attackDamage != previousDamage[i] + increasedAmount) {
					IntegrationTest.Fail (gameObject);
				}
			}
		}
		IntegrationTest.Pass (gameObject);
	}
	
	
}