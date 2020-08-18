using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class HealEnemyMult : BaseSkillMult {

	
	protected GameObject[] enemy;
	protected EnemyHealthMult enemyHealth;
	
	void Awake() {
		skillCost = 90;
		imagePath = "EnemyPotion";
	}
	
	void Start() {
		base.Assign();
	}

	//Kill all the enemy in the scene
	public override void Activate ()
	{
		skillActivation.CmdHealEnemy (skillCost);
	}

}