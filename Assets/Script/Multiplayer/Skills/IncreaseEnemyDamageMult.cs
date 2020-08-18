using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class IncreaseEnemyDamageMult : BaseSkillMult {
	
	public int increasedAmount;
	
	void Awake() {
		skillCost = 80;
		increasedAmount = 10;
		imagePath = "DoubleAxe";
	}
	
	void Start() {
		base.Assign ();
	}
	
	//Kill all the enemy in the scene
	public override void Activate ()
	{
		skillActivation.CmdIncreaseEnemyDamage (increasedAmount, skillCost);
	}
	
	
}
