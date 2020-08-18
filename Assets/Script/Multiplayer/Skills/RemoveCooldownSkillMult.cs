using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class RemoveCooldownSkillMult : BaseSkillMult {
	
	public float reduceTime;
	
	
	void Awake() {
		reduceTime = 7;
		skillCost = 50;
		imagePath = "WingedShoes";
	}
	
	void Start() {
		base.Assign ();
	}
	
	
	public override void Activate ()
	{
		skillActivation.CmdRemoveCooldownSkill (skillCost, reduceTime);
		
	}
	
	
	
}
