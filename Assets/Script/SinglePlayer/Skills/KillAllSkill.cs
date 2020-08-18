using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This skill will kill all enemies on the scene when activated
//All the enemies killed with this skill wont increase player mana
public class KillAllSkill : BaseSkill {
	
	protected GameObject[] enemy;
	protected EnemyHealth enemyHealth;
	
	void Awake() {
		skillCost = 100;
		imagePath = "Skull";
	}
	
	void Start() {
		base.Assign ();
	}
	
	//Kill all the enemy in the scene
	public override void Activate ()
	{
		playerSkill.decreaseBar (skillCost);

		enemy = GameObject.FindGameObjectsWithTag ("Enemy");
		for (int i = 0; i < enemy.Length; i++) {
			enemyHealth = enemy[i].GetComponent<EnemyHealth>();
			enemyHealth.skillValue = 0; //So that when we kill it, our mana doesnt go up
			enemyHealth.Death();
		}
		Debug.Log ("Kill All activated");
		KillAllTest ();
	}
	
	//To test whether there are still enemy left after we activate the skill
	public void KillAllTest() {

		GameObject[] testEnemy;
		EnemyMovement enemyMovement;
		testEnemy = GameObject.FindGameObjectsWithTag ("Enemy");
		//There should be no more enemy
		for (int i = 0; i < testEnemy.Length; i++) {
			enemyMovement = testEnemy [i].GetComponent<EnemyMovement> ();
			if (enemyMovement.speed != 0) {
				IntegrationTest.Fail (gameObject);
			}
		}
		IntegrationTest.Pass (gameObject);
	}
	
	
}