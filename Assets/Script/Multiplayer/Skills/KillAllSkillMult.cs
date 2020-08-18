using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class KillAllSkillMult : BaseSkillMult {
	
	protected GameObject[] enemy;
	
	void Awake() {
		skillCost = 100;
		imagePath = "Skull";
	}
	
	void Start() {
		base.Assign();
	}
	
	//Kill all the enemy in the scene]
	public override void Activate ()
	{
		skillActivation.CmdKillAllSkill (skillCost);
	}
	
}