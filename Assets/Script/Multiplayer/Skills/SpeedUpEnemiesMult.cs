using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class SpeedUpEnemiesMult : BaseSkillMult {
	
	public int plusSpeed;
	
	void Awake() {
		skillCost = 50;
		plusSpeed = 1;
		imagePath = "MonsterWave";
	}
	
	void Start() {
		base.Assign ();
	}
	
	public override void Activate () {
		skillActivation.CmdSpeedUpEnemies (skillCost, plusSpeed);
	}
}
