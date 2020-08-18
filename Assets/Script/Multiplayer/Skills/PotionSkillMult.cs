using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PotionSkillMult : BaseSkillMult  {
	
	public float healPercentage; //How much player HP will be recovered 
	
	void Awake() {
		skillCost = 30;
		healPercentage = 25;
		imagePath = "Potion";
	}
	
	void Start() {
		base.Assign();
	}
	
	//Heal the player and reduce the player mana
	public override void Activate ()
	{
		skillActivation.CmdPotionSkill (healPercentage, skillCost);
	}
	
	
	
	
}
