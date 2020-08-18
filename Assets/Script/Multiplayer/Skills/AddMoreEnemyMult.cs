using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class AddMoreEnemyMult : BaseSkillMult {

	public int plusEnemy;
	
	void Awake() {
		skillCost = 30;
		plusEnemy = 1;
		imagePath = "AMonster";
	}
	
	void Start() {
		base.Assign ();
	}
	
	public override void Activate () {

		skillActivation.CmdAddMoreEnemy (skillCost, plusEnemy);
	}
}