using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class KillOneSideMult : BaseSkillMult {
	
	protected GameObject[] enemy;
	protected EnemyHealthMult enemyHealth;
	
	void Awake() {
		skillCost = 60;
		imagePath = "RocketFist";
	}
	
	void Start() {
		base.Assign();
	}
	
	//Kill all the enemy in the scene
	public override void Activate ()
	{
		skillActivation.CmdKillOneSideSKill (skillCost);
	}

}